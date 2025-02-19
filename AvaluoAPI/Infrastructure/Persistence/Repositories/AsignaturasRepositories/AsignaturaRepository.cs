using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using System.Data;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasRepositories
{
    public class AsignaturaRepository : Repository<Asignatura>, IAsignaturaRepository
    {
        private readonly DapperContext _dapperContext;

        public AsignaturaRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<AsignaturaViewModel?> GetAsignaturaById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
            SELECT 
                a.Id, 
                a.Creditos, 
                a.Codigo, 
                a.Nombre, 
                a.FechaCreacion, 
                a.UltimaEdicion, 
                a.IdEstado, 
                a.ProgramaAsignatura, 
                a.Syllabus, 
                a.IdArea,

                e.Id,
                e.Descripcion,
                e.IdTabla,

                ar.Id,
                ar.Descripcion,
				ar.IdCoordinador,
				ar.FechaCreacion,
				ar.UltimaEdicion
            FROM dbo.asignaturas a
            LEFT JOIN dbo.estado e ON a.IdEstado = e.Id
            LEFT JOIN dbo.areas ar ON a.IdArea = ar.Id
            WHERE a.Id = @Id";

            var parametros = new { Id = id };

            var asignatura = await connection.QueryAsync<AsignaturaViewModel, EstadoViewModel, AreaViewModel, AsignaturaViewModel>(
                query,
                (asignatura, estado, area) =>
                {
                    asignatura.Estado = estado;
                    asignatura.Area = area;
                    return asignatura;
                },
                parametros,
                splitOn: "Id" 
            );

            return asignatura.FirstOrDefault();
        }

        public async Task<PaginatedResult<AsignaturaViewModel>> GetAsignaturas(
            string? codigo,
            string? nombre,
            int? idEstado,
            int? idArea,
            int? page,
            int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
            SELECT COUNT(*)
            FROM dbo.asignaturas a
            LEFT JOIN dbo.estado e ON a.IdEstado = e.Id
            LEFT JOIN dbo.areas ar ON a.IdArea = ar.Id
            WHERE (@Codigo IS NULL OR a.Codigo LIKE '%' + @Codigo + '%')
            AND (@Nombre IS NULL OR a.Nombre LIKE '%' + @Nombre + '%')
            AND (@IdEstado IS NULL OR a.IdEstado = @IdEstado)
            AND (@IdArea IS NULL OR a.IdArea = @IdArea)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                Codigo = codigo,
                Nombre = nombre,
                IdEstado = idEstado,
                IdArea = idArea
            });

            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;
            int offset = (currentPage - 1) * currentRecordsPerPage;

            var query = $@"
            SELECT 
                a.Id, 
                a.Creditos, 
                a.Codigo, 
                a.Nombre, 
                a.FechaCreacion, 
                a.UltimaEdicion, 
                a.IdEstado, 
                a.ProgramaAsignatura, 
                a.Syllabus, 
                a.IdArea,

                e.Id,
                e.Descripcion,
                e.IdTabla,

                ar.Id,
                ar.Descripcion,
				ar.IdCoordinador,
				ar.FechaCreacion,
				ar.UltimaEdicion
            FROM dbo.asignaturas a
            LEFT JOIN dbo.estado e ON a.IdEstado = e.Id
            LEFT JOIN dbo.areas ar ON a.IdArea = ar.Id
            WHERE (@Codigo IS NULL OR a.Codigo LIKE '%' + @Codigo + '%')
            AND (@Nombre IS NULL OR a.Nombre LIKE '%' + @Nombre + '%')
            AND (@IdEstado IS NULL OR a.IdEstado = @IdEstado)
            AND (@IdArea IS NULL OR a.IdArea = @IdArea)
            ORDER BY a.Id
            OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var parametros = new
            {
                Codigo = codigo,
                Nombre = nombre,
                IdEstado = idEstado,
                IdArea = idArea,
                Offset = offset,
                RecordsPerPage = currentRecordsPerPage
            };

            var asignaturasDictionary = new Dictionary<int, AsignaturaViewModel>();

            var asignaturas = await connection.QueryAsync<AsignaturaViewModel, EstadoViewModel, AreaViewModel, AsignaturaViewModel>(
                query,
                (asignatura, estado, area) =>
                {
                    if (!asignaturasDictionary.TryGetValue(asignatura.Id, out var asignaturaEntry))
                    {
                        asignaturaEntry = asignatura;
                        asignaturaEntry.Estado = estado;
                        asignaturaEntry.Area = area;
                        asignaturasDictionary.Add(asignatura.Id, asignaturaEntry);
                    }

                    return asignaturaEntry;
                },
                parametros,
                splitOn: "Id" 
            );

            return new PaginatedResult<AsignaturaViewModel>(asignaturasDictionary.Values, currentPage, currentRecordsPerPage, totalRecords);
        }
    }
}
