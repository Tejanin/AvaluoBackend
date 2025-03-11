using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RolRepositories
{
    public interface IRolRepository : IRepository<Rol>
    {
        Task<RolViewModel?> GetRolById(int id);
        Task<PaginatedResult<RolViewModel>> GetRoles(string? descripcion, int? page, int? recordsPerPage);
    }
}
