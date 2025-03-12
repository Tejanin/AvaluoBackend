using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.CofiguracionViewModels;
using Dapper;
using NuGet.Common;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ConfiguracionRepositories
{
    public class ConfiguracionRepository: Repository<ConfiguracionEvaluaciones>, IConfiguracionRepository
    {
        private readonly DapperContext _dapperContext;

        public ConfiguracionRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<PaginatedResult<ConfiguracionViewModel>> GetConfiguraciones(string? descripcion, DateTime? fechaInicio, DateTime? fechaCierre, int? idEstado, int? page, int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
                        SELECT COUNT(*)
                        FROM ConfiguracionEvaluaciones c  
                        LEFT JOIN estado e ON c.Id_Estado = e.Id
                        WHERE (@Descripcion IS NULL OR c.Descripcion LIKE '%' + @Descripcion + '%')
                        AND (@FechaInicio IS NULL OR c.FechaInicio LIKE '%' + @FechaInicio + '%')
                        AND (@FechaCierre IS NULL OR c.FechaCierre = @FechaCierre)
                        AND (@IdEstado IS NULL OR c.Id_Estado = @IdEstado)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                Descripcion = descripcion,
                FechaInicio = fechaInicio,
                FechaCierre = fechaCierre,
                IdEstado = idEstado,
            });


            if (totalRecords == 0)
            {
                return new PaginatedResult<ConfiguracionViewModel>(Enumerable.Empty<ConfiguracionViewModel>(), 1, 0, 0);
            }

            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;

            int currentPage = page.HasValue && page > 0 ? page.Value : 1;

            int offset = (currentPage - 1) * currentRecordsPerPage;

            var query = $@"
                        SELECT 
                            c.Id, 
                            c.Descripcion,
                            c.FechaInicio,
                            c.FechaCierre,

                            e.Id,
                            e.Descripcion,
                            e.IdTabla
                        FROM ConfiguracionEvaluaciones c
                        LEFT JOIN estado e ON c.Id_Estado = e.Id
                        WHERE (@Descripcion IS NULL OR c.Descripcion LIKE '%' + @Descripcion + '%')
                        AND (@FechaInicio IS NULL OR c.FechaInicio LIKE '%' + @FechaInicio + '%')
                        AND (@FechaCierre IS NULL OR c.FechaCierre = @FechaCierre)
                        AND (@IdEstado IS NULL OR c.Id_Estado = @IdEstado)
                        ORDER BY c.Id
                        OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY
                        ";

            var parametros = new
            {
                Descripcion = descripcion,
                FechaInicio = fechaInicio,
                FechaCierre = fechaCierre,
                IdEstado = idEstado,
                Offset = offset,
                RecordsPerPage = currentRecordsPerPage
            };

            var configuracionDictionary = new Dictionary<int, ConfiguracionViewModel>();

            var configuraciones = await connection.QueryAsync<ConfiguracionViewModel, EstadoViewModel, ConfiguracionViewModel>(
                query,
                (configuracion, estado) =>
                {
                    if (!configuracionDictionary.TryGetValue(configuracion.Id, out var configuracionEntry))
                    {
                        configuracionEntry = configuracion;
                        configuracionEntry.Estado = estado.Descripcion;
                        configuracionDictionary.Add(configuracion.Id, configuracionEntry);
                    }

                    return configuracionEntry;
                },
                parametros,
                splitOn: "Id"
            );
            return new PaginatedResult<ConfiguracionViewModel>(configuracionDictionary.Values, currentPage, currentRecordsPerPage, totalRecords);
        }

        public async Task<ConfiguracionViewModel> GetConfiguracionById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                        SELECT 
                            c.Id, 
                            c.Descripcion,
                            c.FechaInicio,
                            c.FechaCierre,

                            e.Id,
                            e.Descripcion,
                            e.IdTabla
                        FROM ConfiguracionEvaluaciones c 
                        LEFT JOIN estado e ON c.Id_Estado = e.Id
                        WHERE c.Id = @Id";

            var parametros = new { Id = id };

            var configuracion = await connection.QueryAsync<ConfiguracionViewModel, EstadoViewModel, ConfiguracionViewModel>(
                query,
                (configuracion, estado) =>
                {
                    configuracion.Estado = estado.Descripcion;
                    return configuracion;
                },
                parametros,
                splitOn: "Id"
            );

            return configuracion.FirstOrDefault();
        }

        public async Task<FechaConfiguracionViewModel> GetFechaConfiguracion()
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                        SELECT 
                            e.Id
                        FROM estado e
                        WHERE e.IdTabla = 'Configuracion'
                        AND e.Descripcion = 'Activa'";

            var idEstado = await connection.ExecuteScalarAsync<int>(query);

            query = @"
                        SELECT 
                            c.Id,
                            c.FechaInicio,
                            c.FechaCierre
                        FROM ConfiguracionEvaluaciones c
                        WHERE c.Id_Estado = @IdEstado";

            var configuracion = await connection.QueryAsync<FechaConfiguracionViewModel>(
                query,
                new
                {
                    IdEstado = idEstado
                }
            );

            return configuracion.FirstOrDefault();
        }
    }
}
