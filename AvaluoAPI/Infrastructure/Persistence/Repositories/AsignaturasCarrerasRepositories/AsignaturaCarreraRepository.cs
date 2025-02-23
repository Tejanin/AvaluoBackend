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

        public async Task<IEnumerable<AsignaturaCarreraViewModel>> GetAllByCareer(int? idCarrera)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
        SELECT 
            ac.Id_Carrera,
            ac.Id_Asignatura,

            c.Id,
            c.NombreCarrera,

            a.Id,
            a.Nombre
        FROM dbo.asignatura_carrera ac
        INNER JOIN dbo.carreras c ON ac.Id_Carrera = c.Id
        INNER JOIN dbo.asignaturas a ON ac.Id_Asignatura = a.Id
        WHERE (@IdCarrera IS NULL OR ac.Id_Carrera = @IdCarrera)";

            var result = await connection.QueryAsync<AsignaturaCarreraViewModel, CarreraViewModel, AsignaturaViewModel, AsignaturaCarreraViewModel>(
                query,
                (asignaturaCarrera, carrera, asignatura) =>
                {
                    asignaturaCarrera.Carrera = carrera;
                    asignaturaCarrera.Asignatura = asignatura;
                    return asignaturaCarrera;
                },
                new { IdCarrera = idCarrera },
                splitOn: "Id"
            );

            return result;
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
