using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using System.Data;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.InventarioRepositories
{
    public class InventarioRepository : Repository<Inventario>, IInventarioRepository
    {
        private readonly DapperContext _dapperContext;

        public InventarioRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<InventarioViewModel?> GetInventarioById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
            SELECT 
                i.Id, 
                i.Descripcion, 
                i.FechaCreacion, 
                i.UltimaEdicion,
                i.Id_Estado AS IdEstado
            FROM inventario i
            WHERE i.Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<InventarioViewModel>(query, new { Id = id });
        }

        public async Task<PaginatedResult<InventarioViewModel>> GetInventario(string? descripcion, int? page, int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
            SELECT COUNT(*) FROM inventario i
            WHERE (@Descripcion IS NULL OR i.Descripcion LIKE '%' + @Descripcion + '%')";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new { Descripcion = descripcion });

            if (totalRecords == 0)
            {
                return new PaginatedResult<InventarioViewModel>(Enumerable.Empty<InventarioViewModel>(), 1, 0, 0);
            }

            int currentPage = page ?? 1;
            int perPage = recordsPerPage ?? totalRecords;
            int offset = (currentPage - 1) * perPage;

            var query = $@"
            SELECT 
                i.Id, 
                i.Descripcion, 
                i.FechaCreacion, 
                i.UltimaEdicion,
                i.Id_Estado AS IdEstado
            FROM inventario i
            WHERE (@Descripcion IS NULL OR i.Descripcion LIKE '%' + @Descripcion + '%')
            ORDER BY i.Id
            OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var inventario = await connection.QueryAsync<InventarioViewModel>(query, new { Descripcion = descripcion, Offset = offset, RecordsPerPage = perPage });

            return new PaginatedResult<InventarioViewModel>(inventario, currentPage, perPage, totalRecords);
        }

        public async Task<int> GetTotalQuantity(int inventarioId, int? edificioId, int? aulaId)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
            SELECT SUM(cantidad)
            FROM objeto_aula
            WHERE IdInventario = @InventarioId
            AND (@EdificioId IS NULL OR IdEdificio = @EdificioId)
            AND (@AulaId IS NULL OR IdAula = @AulaId)";

            return await connection.ExecuteScalarAsync<int>(query, new { InventarioId = inventarioId, EdificioId = edificioId, AulaId = aulaId });
        }

        public async Task<List<InventarioViewModel>> GetInventarioByLocation(int? edificioId, int? aulaId)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
            SELECT 
                i.Id, 
                i.Descripcion, 
                i.FechaCreacion, 
                i.UltimaEdicion,
                i.Id_Estado AS IdEstado,
                SUM(oa.Cantidad) AS CantidadTotal
            FROM inventario i
            INNER JOIN objeto_aula oa ON i.Id = oa.Id_Objeto
            INNER JOIN aula a ON oa.Id_Aula = a.Id 
            WHERE 
                (@EdificioId IS NULL OR a.Id_Edificio = @EdificioId)
                AND (@AulaId IS NULL OR oa.Id_Aula = @AulaId) 
            GROUP BY i.Id, i.Descripcion, i.FechaCreacion, i.UltimaEdicion, i.Id_Estado";

            var inventario = await connection.QueryAsync<InventarioViewModel>(query, new { EdificioId = edificioId, AulaId = aulaId });

            return inventario.ToList();
        }


    }
}
