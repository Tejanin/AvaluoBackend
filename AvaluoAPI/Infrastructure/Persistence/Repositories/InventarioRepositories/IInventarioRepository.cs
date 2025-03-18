using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.InventarioRepositories
{
    public interface IInventarioRepository : IRepository<Inventario>
    {
        Task<InventarioViewModel?> GetInventarioById(int id);
        Task<PaginatedResult<InventarioViewModel>> GetInventario(string? descripcion, int? page, int? recordsPerPage);
        Task<int> GetTotalQuantity(int inventarioId, int? edificioId, int? aulaId);
        Task<List<InventarioViewModel>> GetInventarioByLocation(int? edificioId, int? aulaId);

    }
}
