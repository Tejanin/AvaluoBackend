using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasCarrerasRepositories
{
    public class AsignaturaCarreraRepository : Repository<AsignaturaCarrera>, IAsignaturaCarreraRepository
    {
        private readonly DapperContext _dapperContext;
        public AsignaturaCarreraRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<PaginatedResult<AsignaturaViewModel>> GetAllByCareer(int idCarrera, int? page, int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            // Obtener el total de registros antes de la paginación
            var countQuery = @"
        SELECT COUNT(*)
        FROM dbo.asignatura_carrera ac
        INNER JOIN dbo.asignaturas a ON ac.Id_Asignatura = a.Id
        WHERE ac.Id_Carrera = @IdCarrera";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new { IdCarrera = idCarrera });

            if (totalRecords == 0)
            {
                return new PaginatedResult<AsignaturaViewModel>(Enumerable.Empty<AsignaturaViewModel>(), 1, 0, 0);
            }

            // Configuración de paginación
            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;
            int offset = (currentPage - 1) * currentRecordsPerPage;

            // Consulta con paginación
            var query = $@"
        SELECT 
            a.Id, 
            a.Creditos, 
            a.Codigo, 
            a.Nombre, 
            a.FechaCreacion, 
            a.UltimaEdicion,  
            a.ProgramaAsignatura, 
            a.Syllabus, 

            e.Id,
            e.Descripcion,
            e.IdTabla,

            ar.Id,
            ar.Descripcion,
            ar.IdCoordinador,
            ar.FechaCreacion,
            ar.UltimaEdicion
        FROM dbo.asignatura_carrera ac
        INNER JOIN dbo.asignaturas a ON ac.Id_Asignatura = a.Id
        LEFT JOIN dbo.estado e ON a.IdEstado = e.Id
        LEFT JOIN dbo.areas ar ON a.IdArea = ar.Id
        WHERE ac.Id_Carrera = @IdCarrera
        ORDER BY a.Id
        OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var parametros = new
            {
                IdCarrera = idCarrera,
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

        public async Task<List<int>> GetCarrerasIdsByAsignaturaId(int asignatura)
        {
            return await _context.Set<AsignaturaCarrera>()
                .Where(ac => ac.IdAsignatura == asignatura)
                .Select(ac => ac.IdCarrera)
                .ToListAsync();
        }
    }
}
