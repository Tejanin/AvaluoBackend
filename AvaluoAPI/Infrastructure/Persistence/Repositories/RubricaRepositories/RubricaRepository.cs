using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
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
    }
}
