using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturaCarreraRepositories
{
    public interface IAsignaturaCarreraRepository : IRepository<AsignaturaCarrera>
    {
        Task<List<int>> GetCarrerasIdsByAsignaturaId(int asignatura);
    }
}
