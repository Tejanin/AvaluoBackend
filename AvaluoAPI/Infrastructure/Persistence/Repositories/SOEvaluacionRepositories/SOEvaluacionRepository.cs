using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories
{
    public class SOEvaluacionRepository: Repository<SOEvaluacion>, ISOEvaluacionRepository
    {
        private readonly DapperContext _dapperContext;
        public SOEvaluacionRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public Task<IEnumerable<SOEvaluacion>> GetSOEvaluaciones()
        {
            throw new NotImplementedException();
        }

        // Obtiene los métodos de evaluación por SO específico
        public async Task<IEnumerable<MetodoEvaluacionViewModel>> GetMetodosEvaluacionPorSO(int idSO)
        {
            using var connection = _dapperContext.CreateConnection();

            const string sql = @"
            SELECT 
                me.Id, 
                me.DescripcionES,
	            me.DescripcionEN,
	            me.UltimaEdicion,
	            me.FechaCreacion
            FROM so_evaluacion soe
            INNER JOIN metodo_evaluacion me ON soe.Id_MetodoEvaluacion = me.Id
            WHERE soe.Id_SO = @idSO";

            var metodos = await connection.QueryAsync<MetodoEvaluacionViewModel>(sql, new { idSO });
            return metodos;
        }

    }

}
