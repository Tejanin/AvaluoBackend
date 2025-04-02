using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasCarrerasRepositories
{
    public interface IAsignaturaCarreraRepository : IRepository<AsignaturaCarrera>
    {
        Task<PaginatedResult<AsignaturaViewModel>> GetAllByCareer(int idCarrera, int? page, int? recordsPerPage);
        Task<List<int>> GetCarrerasIdsByAsignaturaId(int asignatura);
        Task<AsignaturaCarrera> GetByCarreraAsignaturaAsync(int idCarrera, int idAsignatura);
    }
}
