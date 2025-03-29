using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;

using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AreasRepositories
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        private readonly DapperContext _dapperContext;
        public AreaRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<bool> IsCoordinador(int userId)
        {
            var area = await _context.Set<Area>()
                                      .FirstOrDefaultAsync(a => a.IdCoordinador == userId);

            if (area == null)
            {
                return false;
            }

            return true;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }


        public async Task<AreaViewModel> GetAreaById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                        SELECT 
                            a.Id, 
                            a.Descripcion,

                            u.Id,
                            u.Nombre,
                            u.Apellido

                        FROM areas a
                        LEFT JOIN usuario u ON u.Id = a.IdCoordinador 
                        WHERE a.Id = @Id";

            var parametros = new { Id = id };

            var areas = await connection.QueryAsync<AreaViewModel, UsuarioViewModel, AreaViewModel>(
                query,
                (area, usuario) =>
                {
                    if (usuario != null)
                        area.Coordinador = usuario.Nombre + " " + usuario.Apellido;
                    return area;
                },
                parametros,
                splitOn: "Id"
            );

            return areas.FirstOrDefault();
        }

        public async Task<PaginatedResult<AreaViewModel>> GetAreas(string? descripcion, int? idCoordinador, int? page, int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
            SELECT COUNT(*)
            FROM areas a
            LEFT JOIN usuario u ON a.IdCoordinador = u.Id
            WHERE (@Descripcion IS NULL OR a.Descripcion LIKE '%' + @Descripcion + '%')
            AND (@IdCoordinador IS NULL OR a.IdCoordinador = @IdCoordinador)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                Descripcion = descripcion,
                IdCoordinador = idCoordinador
            });

            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;

            int currentPage = page.HasValue && page > 0 ? page.Value : 1;

            int offset = (currentPage - 1) * currentRecordsPerPage;

            var query = $@"
                        SELECT 
                            a.Id, 
                            a.Descripcion,

                            u.Id,
                            u.Nombre,
                            u.Apellido

                        FROM areas a
                        LEFT JOIN usuario u ON u.Id = a.IdCoordinador
                        WHERE (@Descripcion IS NULL OR a.Descripcion LIKE '%' + @Descripcion + '%')
                        AND (@IdCoordinador IS NULL OR a.IdCoordinador = @IdCoordinador)
                        ORDER BY u.Id
                        OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var parametros = new
            {
                Descripcion = descripcion,
                IdCoordinador = idCoordinador,
                Offset = offset,
                RecordsPerPage = currentRecordsPerPage
            };

            var areasDictionary = new Dictionary<int, AreaViewModel>();

            var areas = await connection.QueryAsync<AreaViewModel, UsuarioViewModel, AreaViewModel>(
                query,
                (area, usuario) =>
                {
                    if (!areasDictionary.TryGetValue(area.Id, out var areaEntry))
                    {
                        areaEntry = area;
                        if (usuario != null)
                            areaEntry.Coordinador = usuario.Nombre + " " + usuario.Apellido;
                        areasDictionary.Add(area.Id, areaEntry);
                    }
                    return areaEntry;
                },
                parametros,
                splitOn: "Id"
            );
            return new PaginatedResult<AreaViewModel>(areasDictionary.Values, currentPage, currentRecordsPerPage, totalRecords);

    

        
        }
    }
}
