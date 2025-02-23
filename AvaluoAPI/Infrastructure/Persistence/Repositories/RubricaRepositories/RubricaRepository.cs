using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Dapper;
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

        public async Task<IEnumerable<RubricaViewModel>> GetAllRubricas()
        {
            using var connection = _dapperContext.CreateConnection();

            const string query = @"
                                    SELECT 
                                        r.Id,
                                        r.Comentario,
                                        r.Problematica,
                                        r.Solucion,
                                        r.Evidencia,
                                        r.EvaluacionesFormativas,
                                        r.Estrategias,
                                        rs.Id_PI as IdPI,
                                        rs.CantExperto,
                                        rs.CantSatisfactorio,
                                        rs.CantPrincipiante,
                                        rs.CantDesarrollo
                                    FROM rubricas r
                                    LEFT JOIN resumen rs ON r.Id = rs.Id_Rubrica";

            var rubricaDict = new Dictionary<int, RubricaViewModel>();

            var rubricas = await connection.QueryAsync<RubricaViewModel, ResumenViewModel, RubricaViewModel>(
                query,
                (rubrica, resumen) =>
                {
                    if (!rubricaDict.TryGetValue(rubrica.Id, out var rubricaEntry))
                    {
                        rubricaEntry = rubrica;
                        rubricaEntry.Resumenes = new List<ResumenViewModel>();
                        rubricaDict.Add(rubrica.Id, rubricaEntry);
                    }

                    if (resumen != null)
                    {
                        rubricaEntry.Resumenes.Add(resumen);
                    }

                    return rubricaEntry;
                },
                splitOn: "IdPI");

            return rubricaDict.Values;
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
                       INNER JOIN competencia comp ON r.IdSO = comp.Id
                       INNER JOIN asignaturas a ON r.IdAsignatura = a.Id
                       INNER JOIN estado e ON r.IdEstado = e.Id
                       LEFT JOIN resumen rs ON r.Id = rs.Id_Rubrica
                       WHERE 1=1 ";  // Inicio de condiciones WHERE

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
                splitOn: "Id,Id,Id,Id,IdPI");

            return rubricaDict.Values;
        }
    }
}
