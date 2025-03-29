using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CarrerasRepositories
{
    public class CarreraRepository : Repository<Carrera>, ICarreraRepository
    {
        private readonly DapperContext _dapperContext;

        public CarreraRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }


        public async Task<(bool,string)> IsCoordinador(int userId)
        {
            var carrera = await _context.Set<Carrera>()
                                      .FirstOrDefaultAsync(c => c.IdCoordinadorCarrera == userId);

            if (carrera == null)
            {
                return (false,null);
            }

            return (true,carrera.Id.ToString());
        }
        // Obtener una carrera por ID con su Estado, Área y Coordinador
        public async Task<CarreraViewModel?> GetCarreraById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
            SELECT 
                c.Id, 
                c.Año, 
                c.NombreCarrera, 
                c.PEOs, 
                c.FechaCreacion, 
                c.UltimaEdicion, 
                c.IdEstado, 
                c.IdArea, 
                c.IdCoordinadorCarrera,

                e.Id,
                e.Descripcion,
                e.IdTabla,

                ar.Id,
                ar.Descripcion,
                ar.IdCoordinador,
                ar.FechaCreacion,
                ar.UltimaEdicion,

                u.Id,
                u.Nombre,
				u.Apellido,
                u.Email,
				u.Username
            FROM dbo.carreras c
            LEFT JOIN dbo.estado e ON c.IdEstado = e.Id
            LEFT JOIN dbo.areas ar ON c.IdArea = ar.Id
            LEFT JOIN dbo.usuario u ON c.IdCoordinadorCarrera = u.Id
            WHERE c.Id = @Id";

            var parametros = new { Id = id };

            var carrera = await connection.QueryAsync<CarreraViewModel, EstadoViewModel, AreaViewModel, UsuarioViewModel, CarreraViewModel>(
                query,
                (carrera, estado, area, usuario) =>
                {
                    carrera.Estado = estado;
                    carrera.Area = area;
                    carrera.CoordinadorCarrera = usuario;
                    return carrera;
                },
                parametros,
                splitOn: "Id"
            );

            return carrera.FirstOrDefault();
        }

        // Obtener todas las carreras con filtros y paginación
        public async Task<PaginatedResult<CarreraViewModel>> GetCarreras(
     string? nombreCarrera,
     int? idEstado,
     int? idArea,
     int? idCoordinadorCarrera,
     int? año,
     string? peos,
     int? page,
     int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
    SELECT COUNT(*)
    FROM dbo.carreras c
    LEFT JOIN dbo.estado e ON c.IdEstado = e.Id
    LEFT JOIN dbo.areas ar ON c.IdArea = ar.Id
    LEFT JOIN dbo.usuario u ON c.IdCoordinadorCarrera = u.Id
    WHERE (@NombreCarrera IS NULL OR c.NombreCarrera LIKE '%' + @NombreCarrera + '%')
    AND (@IdEstado IS NULL OR c.IdEstado = @IdEstado)
    AND (@IdArea IS NULL OR c.IdArea = @IdArea)
    AND (@IdCoordinadorCarrera IS NULL OR c.IdCoordinadorCarrera = @IdCoordinadorCarrera)
    AND (@Año IS NULL OR c.Año = @Año)
    AND (@PEOs IS NULL OR c.PEOs LIKE '%' + @PEOs + '%')";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                NombreCarrera = nombreCarrera,
                IdEstado = idEstado,
                IdArea = idArea,
                IdCoordinadorCarrera = idCoordinadorCarrera,
                Año = año,
                PEOs = peos
            });

            if (totalRecords == 0)
            {
                return new PaginatedResult<CarreraViewModel>(Enumerable.Empty<CarreraViewModel>(), 1, 0, 0);
            }

            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;
            int offset = (currentPage - 1) * currentRecordsPerPage;

            var query = $@"
    SELECT 
        c.Id, 
        c.Año, 
        c.NombreCarrera, 
        c.PEOs, 
        c.FechaCreacion, 
        c.UltimaEdicion, 
        c.IdEstado, 
        c.IdArea, 
        c.IdCoordinadorCarrera,

        e.Id,
        e.Descripcion,
        e.IdTabla,

        ar.Id,
        ar.Descripcion,
        ar.IdCoordinador,
        ar.FechaCreacion,
        ar.UltimaEdicion,

        u.Id,
        u.Nombre,
        u.Apellido,
        u.Email,
        u.Username
    FROM dbo.carreras c
    LEFT JOIN dbo.estado e ON c.IdEstado = e.Id
    LEFT JOIN dbo.areas ar ON c.IdArea = ar.Id
    LEFT JOIN dbo.usuario u ON c.IdCoordinadorCarrera = u.Id
    WHERE (@NombreCarrera IS NULL OR c.NombreCarrera LIKE '%' + @NombreCarrera + '%')
    AND (@IdEstado IS NULL OR c.IdEstado = @IdEstado)
    AND (@IdArea IS NULL OR c.IdArea = @IdArea)
    AND (@IdCoordinadorCarrera IS NULL OR c.IdCoordinadorCarrera = @IdCoordinadorCarrera)
    AND (@Año IS NULL OR c.Año = @Año)
    AND (@PEOs IS NULL OR c.PEOs LIKE '%' + @PEOs + '%')
    ORDER BY c.Id
    OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var parametros = new
            {
                NombreCarrera = nombreCarrera,
                IdEstado = idEstado,
                IdArea = idArea,
                IdCoordinadorCarrera = idCoordinadorCarrera,
                Año = año,
                PEOs = peos,
                Offset = offset,
                RecordsPerPage = currentRecordsPerPage
            };

            var carrerasDictionary = new Dictionary<int, CarreraViewModel>();

            var carreras = await connection.QueryAsync<CarreraViewModel, EstadoViewModel, AreaViewModel, UsuarioViewModel, CarreraViewModel>(
                query,
                (carrera, estado, area, usuario) =>
                {
                    if (!carrerasDictionary.TryGetValue(carrera.Id, out var carreraEntry))
                    {
                        carreraEntry = carrera;
                        carreraEntry.Estado = estado;
                        carreraEntry.Area = area;
                        carreraEntry.CoordinadorCarrera = usuario;
                        carrerasDictionary.Add(carrera.Id, carreraEntry);
                    }

                    return carreraEntry;
                },
                parametros,
                splitOn: "Id"
            );

            return new PaginatedResult<CarreraViewModel>(carrerasDictionary.Values, currentPage, currentRecordsPerPage, totalRecords);
        }

    }
}
