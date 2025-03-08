using AvaluoAPI.Presentation.DTOs.InventarioDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.InventarioService
{
    public interface IInventarioService
    {
        Task<PaginatedResult<InventarioViewModel>> GetAll(string? descripcion, int? page, int? recordsPerPage);
        Task<InventarioViewModel> GetById(int id);
        Task Register(InventarioDTO inventarioDTO);
        Task Update(int id, InventarioDTO inventarioDTO);
        Task Delete(int id);
        Task<List<InventarioViewModel>> GetByLocation(int? edificioId, int? aulaId);

    }
}
