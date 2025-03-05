using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using System.Data;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RolRepositories
{
    public class RolRepository : Repository<Rol>, IRolRepository
    {
        private readonly DapperContext _dapperContext;

        public RolRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<RolViewModel?> GetRolById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
            SELECT 
                r.Id, 
                r.Descripcion, 
                r.EsProfesor,
                r.EsSupervisor,
                r.EsCoordinadorArea,
                r.EsCoordinadorCarrera,
                r.EsAdmin,
                r.EsAux,
                r.VerInformes,
                r.VerListaDeRubricas,
                r.ConfigurarFechas,
                r.VerManejoCurriculum
            FROM roles r
            WHERE r.Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<RolViewModel>(query, new { Id = id });
        }

        public async Task<PaginatedResult<RolViewModel>> GetRoles(string? descripcion, int? page, int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
            SELECT COUNT(*) FROM roles r
            WHERE (@Descripcion IS NULL OR r.Descripcion LIKE '%' + @Descripcion + '%')";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new { Descripcion = descripcion });

            if (totalRecords == 0)
            {
                return new PaginatedResult<RolViewModel>(Enumerable.Empty<RolViewModel>(), 1, 0, 0);
            }

            int currentPage = page ?? 1;
            int perPage = recordsPerPage ?? totalRecords;
            int offset = (currentPage - 1) * perPage;

            var query = $@"
            SELECT 
                r.Id, 
                r.Descripcion, 
                r.EsProfesor,
                r.EsSupervisor,
                r.EsCoordinadorArea,
                r.EsCoordinadorCarrera,
                r.EsAdmin,
                r.EsAux,
                r.VerInformes,
                r.VerListaDeRubricas,
                r.ConfigurarFechas,
                r.VerManejoCurriculum
            FROM roles r
            WHERE (@Descripcion IS NULL OR r.Descripcion LIKE '%' + @Descripcion + '%')
            ORDER BY r.Id
            OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var roles = await connection.QueryAsync<RolViewModel>(query, new { Descripcion = descripcion, Offset = offset, RecordsPerPage = perPage });

            return new PaginatedResult<RolViewModel>(roles, currentPage, perPage, totalRecords);
        }
    }
}
