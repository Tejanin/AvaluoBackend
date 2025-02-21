using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AulaRepositories
{
    public class AulaRepository: Repository<Aula>, IAulaRepository
    {
        private readonly DapperContext _dapperContext;

        public AulaRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<IEnumerable<AulaViewModel>> GetAulas(string? descripcion, int? idEdificio, int? idEstado)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = $@"
    SELECT COUNT(*)
    FROM aula a
    LEFT JOIN edificios e ON a.Id_Edificio = e.Id  
    LEFT JOIN estado es ON a.Id_Estado = es.Id
    WHERE (@Descripcion IS NULL OR a.Descripcion LIKE '%' + @Descripcion + '%')
    AND (@IdEdificio IS NULL OR a.Id_Edificio = @IdEdificio)
    AND (@IdEstado IS NULL OR a.Id_Estado = @IdEstado)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                Descripcion = descripcion,
                IdEdificio = idEdificio,
                IdEstado = idEstado
            });

            var query = $@"
        SELECT 
        a.Id, 
        a.Descripcion,
        a.FechaCreacion, 
        a.UltimaEdicion,

        e.Id,
        e.Nombre,
        e.Acron,
        e.Ubicacion,
        (SELECT COUNT(*) FROM aula a2 WHERE a2.Id_Edificio = e.Id) CantAulas,
        
        eds.Id,
        eds.IdTabla,
        eds.Descripcion,

        ar.Id,
        ar.Descripcion,

        es.Id,
        es.IdTabla,
        es.Descripcion

    FROM aula a
    LEFT JOIN edificios e ON a.Id_Edificio = e.Id  
    LEFT JOIN estado es ON a.Id_Estado = es.Id
    LEFT JOIN estado eds ON e.Id_Estado = eds.Id
    LEFT JOIN areas ar ON e.Id_Area = ar.Id
    WHERE (@Descripcion IS NULL OR a.Descripcion LIKE '%' + @Descripcion + '%')
    AND (@IdEdificio IS NULL OR a.Id_Edificio = @IdEdificio)
    AND (@IdEstado IS NULL OR a.Id_Estado = @IdEstado)
    ORDER BY a.Id";

            var parametros = new
            {
                Descripcion = descripcion,
                IdEdificio = idEdificio,
                IdEstado = idEstado,
            };

            var aulasDictionary = new Dictionary<int, AulaViewModel>();

            var aulas = await connection.QueryAsync<AulaViewModel, EdificioViewModel, EstadoViewModel, EstadoViewModel, AreaViewModel, AulaViewModel>(
                query,
                (aula, edificio, estado, edificioEstado, area) =>
                {
                    if (!aulasDictionary.TryGetValue(aula.Id, out var aulaEntry))
                    {
                        aulaEntry = aula;
                        aulaEntry.Edificio = edificio;
                        aulaEntry.Estado = estado;
                        aulaEntry.Edificio.Area = area.Descripcion;
                        aulaEntry.Edificio.Estado = edificioEstado.Descripcion;
                        aulasDictionary.Add(aula.Id, aulaEntry);
                    }

                    return aulaEntry;
                },
                parametros,
                splitOn: "Id"
            );

            return aulasDictionary.Values;

        }

        public async Task<AulaViewModel> GetAulasById(int id)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
    SELECT 
        a.Id, 
        a.Descripcion,
        a.FechaCreacion, 
        a.UltimaEdicion,

        e.Id,
        e.Nombre,
        e.Acron,
        e.Ubicacion,
        (SELECT COUNT(*) FROM aula a2 WHERE a2.Id_Edificio = e.Id) CantAulas,
        
        eds.Id,
        eds.IdTabla,
        eds.Descripcion,

        ar.Id,
        ar.Descripcion,

        es.Id,
        es.IdTabla,
        es.Descripcion

    FROM aula a
    LEFT JOIN edificios e ON a.Id_Edificio = e.Id  
    LEFT JOIN estado es ON a.Id_Estado = es.Id
    LEFT JOIN estado eds ON e.Id_Estado = eds.Id
    LEFT JOIN areas ar ON e.Id_Area = ar.Id
    WHERE a.Id = @Id";

            var parametros = new { Id = id };

            var aula = await connection.QueryAsync<AulaViewModel, EdificioViewModel, EstadoViewModel, EstadoViewModel, AreaViewModel, AulaViewModel>(
                query,
                (aula, edificio, estado, estadoEdificio, area) =>
                {
                    aula.Edificio = edificio;
                    aula.Edificio.Estado = estadoEdificio.Descripcion;
                    aula.Edificio.Area = area.Descripcion;
                    aula.Estado = estado;
                    return aula;
                },
                parametros,
                splitOn: "Id,Id,Id,Id,Id"
            );

            return aula.FirstOrDefault();

        }
    }
}
