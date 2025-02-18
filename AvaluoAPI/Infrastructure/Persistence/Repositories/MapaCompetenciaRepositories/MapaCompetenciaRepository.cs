using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.MapaCompetenciaRepositories
{
    public class MapaCompetenciaRepository : Repository<MapaCompetencias>, IMapaCompetenciaRepository 
    {
        private readonly DapperContext _dapperContext;
        public MapaCompetenciaRepository(AvaluoDbContext context, DapperContext dapperContext) : base(context)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetAsignaturasConCompetencias()
        {
            using var connection = _dapperContext.CreateConnection();

            const string query = @"
                                   SELECT 
                                        a.Id,
                                        a.Codigo,
                                        a.Nombre,
                                        ea.Descripcion as Estado,
                                        c.Id,
                                        c.Nombre,
                                        c.Acron,
                                        ec.Descripcion as Estado
                                    FROM asignaturas a
                                    INNER JOIN mapa_competencias mc ON a.Id = mc.Id_Asignatura
                                    INNER JOIN competencia c ON mc.Id_Competencia = c.Id
                                    INNER JOIN estado ea ON a.IdEstado = ea.Id
                                    INNER JOIN estado ec ON c.Id_Estado = ec.Id
                                    INNER JOIN estado em ON mc.Id_Estado = em.Id
                                    INNER JOIN tipo_competencia tc ON c.Id_Tipo = tc.Id
                                    WHERE em.Descripcion = 'Activada'
                                    AND tc.Nombre = N'Específica'";

            var lookup = new List<AsignaturaConCompetenciasViewModel>();
            var asignaturasDict = new Dictionary<int, AsignaturaConCompetenciasViewModel>();

            await connection.QueryAsync<AsignaturaConCompetenciasViewModel, CompetenciaResumenViewModel, AsignaturaConCompetenciasViewModel>(
                query,
                (asignatura, competencia) =>
                {
                    if (!asignaturasDict.TryGetValue(asignatura.Id, out var asignaturaExistente))
                    {
                        asignaturaExistente = asignatura;
                        asignaturaExistente.Competencias = new List<CompetenciaResumenViewModel>();
                        asignaturasDict.Add(asignatura.Id, asignaturaExistente);
                        lookup.Add(asignaturaExistente);
                    }

                    asignaturaExistente.Competencias.Add(competencia);
                    return asignaturaExistente;
                },
                splitOn: "Id");

            return lookup;
        }
    }
}
