using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasCarrerasRepositories
{
    public interface IAsignaturaCarreraRepository : IRepository<AsignaturaCarrera>
    {
        Task<IEnumerable<AsignaturaCarreraViewModel>> GetAllByCareer(int? idCarrera);
    }
}
