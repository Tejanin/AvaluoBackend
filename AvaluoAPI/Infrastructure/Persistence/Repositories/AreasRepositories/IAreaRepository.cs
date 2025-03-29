using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories
{
    public interface IAreaRepository : IRepository<Area>
    {

        Task<AreaViewModel> GetAreaById(int id);
        Task<PaginatedResult<AreaViewModel>> GetAreas(string? descripcion, int? idCoordinador, int? page, int? recordsPerPage);
        Task<bool> IsCoordinador(int userId);
    }
}
