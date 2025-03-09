using Avaluo.Infrastructure.Data;

using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.DashboardsViewModels.AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;



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

            // Obtener rúbricas básicas
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
                            r.Estrategias
                        FROM rubricas r
                        INNER JOIN asignaturas a ON r.IdAsignatura = a.Id
                        INNER JOIN estado e ON r.IdEstado = e.Id
                        WHERE r.IdProfesor = @IdProfesor
                          AND r.IdEstado IN (@IdEstado1, @IdEstado2)
                        ORDER BY r.IdAsignatura, r.Seccion";

            var parameters = new
            {
                IdProfesor = profesor,
                IdEstado1 = activo,
                IdEstado2 = activoSinEntregar
            };

            var rubricas = (await connection.QueryAsync<RubricaDashboardViewModel>(rubricasQuery, parameters)).ToList();

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
                        rubrica.SO = null;
                    }

                    // Inicializar lista de resúmenes vacía
                    rubrica.Resumenes = new List<ResumenViewModelMixed>();
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
                        rubrica.Resumenes.Add(new ResumenViewModelMixed
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
        public async Task<PaginatedResult<RubricaViewModel>> GetRubricasFiltered(int? idSO = null, List<int>? carrerasIds = null, int? idEstado = null, int? idAsignatura = null, int? page = null, int? recordsPerPage = null)


        {
            using var connection = _dapperContext.CreateConnection();

            // Base del query para contar el total de registros
            var countQuery = @"
        SELECT COUNT(*)
        FROM rubricas r
        INNER JOIN carrera_rubrica cr ON r.Id = cr.Id_Rubrica
        INNER JOIN carreras c ON cr.Id_Carrera = c.Id
        INNER JOIN usuario u ON r.IdProfesor = u.Id
        INNER JOIN competencia comp ON r.IdSO = comp.Id
        INNER JOIN asignaturas a ON r.IdAsignatura = a.Id
        INNER JOIN estado e ON r.IdEstado = e.Id
        LEFT JOIN resumen rs ON r.Id = rs.Id_Rubrica
        WHERE 1=1 ";

            // Query principal con JOINs y datos detallados
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
                       WHERE 1=1 ";  // Inicio de condiciones WHERE

            var parameters = new DynamicParameters();

            if (idSO.HasValue)
            {
                countQuery += " AND r.IdSO = @IdSO";
                query += " AND r.IdSO = @IdSO";
                parameters.Add("IdSO", idSO.Value);
            }

            if (carrerasIds != null && carrerasIds.Any())
            {
                countQuery += " AND cr.Id_Carrera IN @CarrerasIds";
                query += " AND cr.Id_Carrera IN @CarrerasIds";
                parameters.Add("CarrerasIds", carrerasIds);
            }

            if (idEstado.HasValue)
            {
                countQuery += " AND r.IdEstado = @IdEstado";
                query += " AND r.IdEstado = @IdEstado";
                parameters.Add("IdEstado", idEstado.Value);
            }

            if (idAsignatura.HasValue)
            {
                countQuery += " AND r.IdAsignatura = @IdAsignatura";
                query += " AND r.IdAsignatura = @IdAsignatura";
                parameters.Add("IdAsignatura", idAsignatura.Value);
            }

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, parameters);

            if (totalRecords == 0)
            {
                return new PaginatedResult<RubricaViewModel>(Enumerable.Empty<RubricaViewModel>(), 1, 0, 0);
            }

            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;
            int offset = (currentPage - 1) * currentRecordsPerPage;

            // Agregar paginación al query principal
            query += " ORDER BY r.Id OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";
            parameters.Add("Offset", offset);
            parameters.Add("RecordsPerPage", currentRecordsPerPage);

            var rubricaDict = new Dictionary<string, RubricaViewModel>();

            var rubricas = await connection.QueryAsync<RubricaViewModel, CarreraRubricaViewModel, SORubricaViewModel, AsignaturaRubricaViewModel, EstadoViewModel, ResumenViewModel, RubricaViewModel>(
                query,
                (rubrica, carrera, so, asignatura, estado, resumen) =>
                {
                    var key = $"{rubrica.Id}_{carrera.Id}";

                    if (!rubricaDict.TryGetValue(key, out var rubricaEntry))
                    {
                        rubricaEntry = rubrica;
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
                splitOn: "Id,Id,Id,Id,Id,IdPI");

            return new PaginatedResult<RubricaViewModel>(rubricaDict.Values, currentPage, currentRecordsPerPage, totalRecords);
        }

        public async Task<List<ReporteSOViewModel>> ObtenerTableroDeSO(int idCarrera, int año, string periodo)
        {
            using var connection = _dapperContext.CreateConnection();
            {
                var query = @"
                    SELECT 
                        car.Id AS CarreraId,
                        car.NombreCarrera AS Carrera,
                        c.Id AS SO_Id,
                        c.Nombre AS SO_Nombre,
                        p.Id AS PI_Id,
                        p.Nombre AS PI_Nombre,
                        SUM(r.CantEstudiantes) AS CantidadTotalEstudiantes,
                        SUM(res.CantPrincipiante) AS CantPrincipiante,
                        SUM(res.CantDesarrollo) AS CantDesarrollo,
                        SUM(res.CantSatisfactorio) AS CantSatisfactorio,
                        SUM(res.CantExperto) AS CantExperto,
                        CAST(SUM(res.CantPrincipiante) AS FLOAT) / NULLIF(SUM(r.CantEstudiantes), 0) * 100 AS PorcentajePrincipiante,
                        CAST(SUM(res.CantDesarrollo) AS FLOAT) / NULLIF(SUM(r.CantEstudiantes), 0) * 100 AS PorcentajeDesarrollo,
                        CAST(SUM(res.CantSatisfactorio) AS FLOAT) / NULLIF(SUM(r.CantEstudiantes), 0) * 100 AS PorcentajeSatisfactorio,
                        CAST(SUM(res.CantExperto) AS FLOAT) / NULLIF(SUM(r.CantEstudiantes), 0) * 100 AS PorcentajeExperto
                    FROM 
                        rubricas r
                    INNER JOIN 
                        resumen res ON r.Id = res.Id_Rubrica
                    INNER JOIN 
                        competencia c ON r.IdSO = c.Id
                    INNER JOIN 
                        pi p ON res.Id_PI = p.Id AND p.SO_Id = c.Id
                    INNER JOIN 
                        asignatura_carrera ac ON r.IdAsignatura = ac.Id_Asignatura
                    INNER JOIN 
                        carreras car ON ac.Id_Carrera = car.Id
                    WHERE 
                        r.IdEstado = 2
                        AND r.Año = @Año
                        AND r.Periodo = @Periodo
                        AND ac.Id_Carrera = @IdCarrera
                    GROUP BY 
                        car.Id,
                        car.NombreCarrera,
                        c.Id, 
                        c.Nombre,
                        p.Id,
                        p.Nombre
                    ORDER BY 
                        c.Id, 
                        p.Id;";

                var parameters = new
                {
                    IdCarrera = idCarrera,
                    Año = año,
                    Periodo = periodo
                };

                // Ejecutamos la consulta y obtenemos los resultados sin procesar
                var rawData = await connection.QueryAsync<dynamic>(query, parameters);

                // Si no hay datos, devolvemos un objeto vacío
                if (!rawData.Any())
                {
                    return new List<ReporteSOViewModel> {
                        new ReporteSOViewModel
                        {
                            Carrera = "No hay datos disponibles",
                            Resumen = new List<SOResumenViewModel>()
                        }
                    };
                }

                // Construimos la estructura jerárquica
                var reporte = new ReporteSOViewModel
                {
                    Carrera = rawData.First().Carrera
                };

                // Agrupar por SO
                var soGroups = rawData.GroupBy(r => new { Id = (int)r.SO_Id, Nombre = (string)r.SO_Nombre });

                foreach (var soGroup in soGroups)
                {
                    var soResumen = new SOResumenViewModel
                    {
                        SO = soGroup.Key.Nombre,
                        // Sumamos el total de estudiantes de todos los PIs para este SO
                        CantidadTotalEstudiantes = soGroup.Sum(r => (int)r.CantidadTotalEstudiantes)
                    };

                    // Procesamos cada PI dentro de este SO
                    foreach (var row in soGroup)
                    {
                        var piResumen = new PIResumenViewModel
                        {
                            PI = row.PI_Nombre,
                            CantidadTotalEstudiantes = row.CantidadTotalEstudiantes,
                            CantPrincipiante = row.CantPrincipiante,
                            CantDesarrollo = row.CantDesarrollo,
                            CantSatisfactorio = row.CantSatisfactorio,
                            CantExperto = row.CantExperto,
                            PorcentajePrincipiante = Math.Round((decimal)row.PorcentajePrincipiante, 2),
                            PorcentajeDesarrollo = Math.Round((decimal)row.PorcentajeDesarrollo, 2),
                            PorcentajeSatisfactorio = Math.Round((decimal)row.PorcentajeSatisfactorio, 2),
                            PorcentajeExperto = Math.Round((decimal)row.PorcentajeExperto, 2)
                        };

                        soResumen.PIs.Add(piResumen);
                    }

                    reporte.Resumen.Add(soResumen);
                }

                return new List<ReporteSOViewModel> { reporte };
            }
        }

        public async Task<List<ReporteSOViewModel>> ObtenerTableroDeSOPorArea(int area, int año, string periodo)
        {
            // Consulta para obtener los IDs de carreras que pertenecen a un área específica
            var query = "SELECT Id FROM carreras WHERE IdArea = @Area AND IdEstado = 1"; // Asumiendo IdEstado = 1 para carreras activas

            // Obtener los IDs de carreras usando el DapperContext inyectado
            using (var connection = _dapperContext.CreateConnection())
            {
                // Ejecutar la consulta para obtener los IDs de carreras
                var carrerasIds = await connection.QueryAsync<int>(query, new { Area = area });

                // Crear la lista para almacenar los resultados
                var tablasSO = new List<ReporteSOViewModel>();

                // Procesar cada carrera
                foreach (int id in carrerasIds)
                {
                    var reporte = await ObtenerTableroDeSO(id, año, periodo);
                    if (reporte != null)
                    {
                        tablasSO.AddRange(reporte);
                    }
                }

                return tablasSO;
            }
        }
    }
}
