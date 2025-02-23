using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories
{
    public interface IAreaRepository : IRepository<Area>
    {
       Task<IEnumerable<AreaViewModel>> GetAllAreas();
    }
}
