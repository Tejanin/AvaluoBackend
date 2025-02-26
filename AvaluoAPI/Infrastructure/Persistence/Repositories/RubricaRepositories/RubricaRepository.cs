﻿using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories
{
    public class RubricaRepository: Repository<Rubrica>,IRubricaRepository
    {
        private readonly DapperContext _dapperContext;
        public RubricaRepository(AvaluoDbContext context, DapperContext dapperContext): base(context)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<List<SeccionRubricasViewModel>> GetProfesorSeccionesWithRubricas(int profesor, int activo, int activoSinEntregar)
        {
            using var connection = _dapperContext.CreateConnection();

            // Obtener rúbricas básicas con método de evaluación
            var rubricasQuery = @"
        SELECT 
            r.Id,
            r.IdSO,
            r.Seccion,
            r.IdAsignatura,
            a.Codigo AS AsignaturaCodigo,
            a.Nombre AS AsignaturaNombre,
            e.Descripcion AS Estado,
            r.Comentario,
            r.Problematica,
            r.Solucion,
            r.Evidencia,
            r.EvaluacionesFormativas,
            r.Estrategias,
            me.Id AS MetodoEvaluacion_Id,
            me.DescripcionES AS MetodoEvaluacion_DescripcionES,
            me.DescripcionEN AS MetodoEvaluacion_DescripcionEN
        FROM rubricas r
        INNER JOIN asignaturas a ON r.IdAsignatura = a.Id
        INNER JOIN estado e ON r.IdEstado = e.Id
        LEFT JOIN metodo_evaluacion me ON r.MetodoEvaluacion = me.Id
        WHERE r.IdProfesor = @IdProfesor
          AND r.IdEstado IN (@IdEstado1, @IdEstado2)
        ORDER BY r.IdAsignatura, r.Seccion";

            var parameters = new
            {
                IdProfesor = profesor,
                IdEstado1 = activo,
                IdEstado2 = activoSinEntregar
            };

            var rubricas = new List<RubricaDashboardViewModel>();

            await connection.QueryAsync<RubricaDashboardViewModel, MetodoEvaluacionViewModel, RubricaDashboardViewModel>(
                rubricasQuery,
                (rubrica, metodoEvaluacion) =>
                {
                    rubrica.MetodoEvaluacion = metodoEvaluacion;
                    rubricas.Add(rubrica);
                    return rubrica;
                },
                parameters,
                splitOn: "MetodoEvaluacion_Id"
            );

            if (rubricas.Any())
            {
                // Obtener SO y PIs para cada rúbrica
                var soIds = rubricas.Select(r => r.Id).Distinct().ToList();
                var soQuery = @"
            SELECT 
                c.Id,
                c.Nombre,
                c.Acron,
                c.DescripcionES,
                p.Id AS PI_Id,
                p.Nombre AS PI_Nombre,
                p.DescripcionES AS PI_DescripcionES,
                p.DescripcionEN AS PI_DescripcionEN
            FROM rubricas r
            INNER JOIN competencia c ON r.IdSO = c.Id
            LEFT JOIN pi p ON c.Id = p.SO_Id
            WHERE r.Id IN @RubricaIds";

                // Diccionario para mapear rúbricas a sus SOs
                var soDictionary = new Dictionary<int, SOwithPIsViewModel>();
                var rubricaToSoDict = new Dictionary<int, int>();

                // Obtener relación entre rúbricas y SOs
                var rubricaSoQuery = @"
            SELECT Id, IdSO FROM rubricas WHERE Id IN @RubricaIds";
                var rubricaSoRelations = await connection.QueryAsync(rubricaSoQuery, new { RubricaIds = soIds });

                foreach (var relation in rubricaSoRelations)
                {
                    rubricaToSoDict[(int)relation.Id] = (int)relation.IdSO;
                }

                // Obtener datos de SOs y PIs
                var soResults = await connection.QueryAsync(soQuery, new { RubricaIds = soIds });

                foreach (var row in soResults)
                {
                    var soId = (int)row.Id;

                    if (!soDictionary.ContainsKey(soId))
                    {
                        soDictionary[soId] = new SOwithPIsViewModel
                        {
                            Id = soId,
                            Nombre = row.Nombre,
                            Acron = row.Acron,
                            DescripcionES = row.DescripcionES,
                            PIs = new List<PIViewModel>()
                        };
                    }

                    // Agregar PI si existe
                    if (row.PI_Id != null)
                    {
                        var pi = new PIViewModel
                        {
                            Id = (int)row.PI_Id,
                            Nombre = row.PI_Nombre,
                            DescripcionES = row.PI_DescripcionES,
                            DescripcionEN = row.PI_DescripcionEN
                        };

                        // Verificar que no exista ya un PI con el mismo ID
                        if (!soDictionary[soId].PIs.Any(p => p.Id == pi.Id))
                        {
                            soDictionary[soId].PIs.Add(pi);
                        }
                    }
                }

                // Asignar SOs a rúbricas
                foreach (var rubrica in rubricas)
                {
                    if (rubricaToSoDict.ContainsKey(rubrica.Id) &&
                        soDictionary.ContainsKey(rubricaToSoDict[rubrica.Id]))
                    {
                        rubrica.SO = soDictionary[rubricaToSoDict[rubrica.Id]];
                    }
                    else
                    {
                        // SO por defecto en caso de no encontrar uno
                        rubrica.SO = new SOwithPIsViewModel
                        {
                            Id = 0,
                            Nombre = string.Empty,
                            Acron = string.Empty,
                            DescripcionES = string.Empty,
                            PIs = new List<PIViewModel>()
                        };
                    }

                    // Inicializar lista de resúmenes vacía
                    rubrica.Resumenes = new List<ResumenViewModel>();
                }

                // Obtener resúmenes para cada rúbrica
                var rubricaIds = rubricas.Select(r => r.Id).ToList();
                var resumenQuery = @"
            SELECT 
                Id_PI AS IdPI,
                Id_Rubrica,
                CantExperto,
                CantSatisfactorio,
                CantPrincipiante,
                CantDesarrollo
            FROM resumen
            WHERE Id_Rubrica IN @RubricaIds";

                var resumenes = await connection.QueryAsync(resumenQuery, new { RubricaIds = rubricaIds });

                // Asignar resúmenes a rúbricas
                foreach (var resumen in resumenes)
                {
                    var rubricaId = (int)resumen.Id_Rubrica;
                    var rubrica = rubricas.FirstOrDefault(r => r.Id == rubricaId);

                    if (rubrica != null)
                    {
                        rubrica.Resumenes.Add(new ResumenViewModel
                        {
                            IdPI = (int)resumen.IdPI,
                            CantExperto = (int)resumen.CantExperto,
                            CantSatisfactorio = (int)resumen.CantSatisfactorio,
                            CantPrincipiante = (int)resumen.CantPrincipiante,
                            CantDesarrollo = (int)resumen.CantDesarrollo
                        });
                    }
                }
            }

            // Agrupar por Asignatura + Sección
            var resultado = rubricas
                .GroupBy(r => new { r.IdAsignatura, r.Seccion })
                .Select(g => new SeccionRubricasViewModel
                {
                    Seccion = g.Key.Seccion,
                    Asignatura = $"{g.First().AsignaturaCodigo} - {g.First().AsignaturaNombre}",
                    Rubricas = g.ToList()
                })
                .ToList();

            return resultado;
        }

        public async Task<List<int>> ObtenerIdAsignaturasPorEstadoAsync(int idEstado)
        {
            using var connection = _dapperContext.CreateConnection();
            string query = "SELECT DISTINCT IdAsignatura FROM rubricas WHERE IdEstado = @IdEstado;";

            var asignaturas = await connection.QueryAsync<int>(query, new { IdEstado = idEstado });

            return asignaturas.ToList();
        }
        public async Task<IEnumerable<RubricaViewModel>> GetRubricasFiltered(int? idSO = null, List<int>? carrerasIds = null, int? idEstado = null, int? idAsignatura = null)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                        SELECT 
                   r.Id,
                   r.Comentario,
                   r.Problematica,
                   r.Solucion,
                   r.Evidencia,
                   r.EvaluacionesFormativas,
                   r.Estrategias,
                   CONCAT(u.Nombre, ' ', u.Apellido) AS Profesor,    
                   c.Id,
                   c.NombreCarrera as Nombre,
                   comp.Id,
                   comp.Nombre,
                   comp.Acron,
                   a.Id,
                   a.Codigo,
                   a.Nombre,
                   e.Id,
                   e.IdTabla,
                   e.Descripcion,
                   rs.Id_PI as IdPI,
                   rs.CantExperto,
                   rs.CantSatisfactorio,
                   rs.CantPrincipiante,
                   rs.CantDesarrollo
               FROM rubricas r
               INNER JOIN carrera_rubrica cr ON r.Id = cr.Id_Rubrica
               INNER JOIN carreras c ON cr.Id_Carrera = c.Id
               INNER JOIN usuario u ON r.IdProfesor = u.Id
               INNER JOIN competencia comp ON r.IdSO = comp.Id
               INNER JOIN asignaturas a ON r.IdAsignatura = a.Id
               INNER JOIN estado e ON r.IdEstado = e.Id
               LEFT JOIN resumen rs ON r.Id = rs.Id_Rubrica
               WHERE 1=1  ";  // Inicio de condiciones WHERE

            var parameters = new DynamicParameters();

            if (idSO.HasValue)
            {
                query += " AND r.IdSO = @IdSO";
                parameters.Add("IdSO", idSO.Value);
            }

            if (carrerasIds != null && carrerasIds.Any())
            {
                query += " AND cr.Id_Carrera IN @CarrerasIds";
                parameters.Add("CarrerasIds", carrerasIds);
            }

            if (idEstado.HasValue)
            {
                query += " AND r.IdEstado = @IdEstado";
                parameters.Add("IdEstado", idEstado.Value);
            }

            if (idAsignatura.HasValue)
            {
                query += " AND r.IdAsignatura = @IdAsignatura";
                parameters.Add("IdAsignatura", idAsignatura.Value);
            }

            var rubricaDict = new Dictionary<string, RubricaViewModel>();
            var rubricas = await connection.QueryAsync<RubricaViewModel, CarreraRubricaViewModel, SORubricaViewModel, AsignaturaRubricaViewModel, EstadoViewModel, ResumenViewModel, RubricaViewModel>(
                query,
                (rubrica, carrera, so, asignatura, estado, resumen) =>
                {
                    var key = $"{rubrica.Id}_{carrera.Id}";
                    if (!rubricaDict.TryGetValue(key, out var rubricaEntry))
                    {
                        rubricaEntry = rubrica;  // Profesor ya está asignado en el objeto rubrica
                        rubricaEntry.Carrera = carrera;
                        rubricaEntry.SO = so;
                        rubricaEntry.Asignatura = asignatura;
                        rubricaEntry.Estado = estado;
                        rubricaEntry.Resumenes = new List<ResumenViewModel>();
                        rubricaDict.Add(key, rubricaEntry);
                    }
                    if (resumen != null)
                    {
                        rubricaEntry.Resumenes.Add(resumen);
                    }
                    return rubricaEntry;
                },
                parameters,
                splitOn: "Id,Id,Id,Id,IdPI");

            return rubricaDict.Values;
        }
    }
}
