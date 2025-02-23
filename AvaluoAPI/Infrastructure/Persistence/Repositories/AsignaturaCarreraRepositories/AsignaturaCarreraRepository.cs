using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturaCarreraRepositories
{
    public class AsignaturaCarreraRepository : Repository<AsignaturaCarrera>, IAsignaturaCarreraRepository
    {
        public AsignaturaCarreraRepository(AvaluoDbContext context): base(context)
        {

        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
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
