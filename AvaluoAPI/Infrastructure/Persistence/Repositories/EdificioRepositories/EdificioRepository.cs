using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories
{
    public class EdificioRepository: Repository<Edificio>, IEdificioRespository
    {
        private readonly DapperContext _dapperContext;
        public EdificioRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<EdificioViewModel>> GetAllEdificios()
        {
            using var connection = _dapperContext.CreateConnection();
            var query = @"
                    SELECT 
                        e.Id,
                        e.Nombre,
                        (SELECT COUNT(*) FROM aula a2 WHERE a2.Id_Edificio = e.Id) CantAulas,
                        e.Acron,
                        e.Ubicacion,
                        a.Descripcion AS Area,
                        es.Descripcion AS Estado
                    FROM edificios e
                    LEFT JOIN Areas a ON e.Id_Area = a.Id
                    LEFT JOIN Estado es ON e.Id_Estado = es.Id;";
            return await connection.QueryAsync<EdificioViewModel>(query);
        }

        public async Task<EdificioViewModel> GetEdificioById(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = @"
                    SELECT 
                        e.Id,
                        e.Nombre,
                        (SELECT COUNT(*) FROM aula a2 WHERE a2.Id_Edificio = e.Id) CantAulas,
                        e.Acron,
                        e.Ubicacion,
                        a.Descripcion AS Area,
                        es.Descripcion AS Estado
                    FROM edificios e
                    LEFT JOIN Areas a ON e.Id_Area = a.Id
                    LEFT JOIN Estado es ON e.Id_Estado = es.Id
                    WHERE e.Id = @Id;";
            var parametros = new { Id = id };
            return await connection.QuerySingleOrDefaultAsync<EdificioViewModel>(query, parametros);
        }
    }
}
