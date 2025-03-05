using AvaluoAPI.Presentation.DTOs.RolDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.RolService
{
    public interface IRolService
    {
        Task<PaginatedResult<RolViewModel>> GetAll(string? descripcion, int? page, int? recordsPerPage);
        Task<RolViewModel> GetById(int id);
        Task Register(RolDTO rolDTO);
        Task Update(int id, RolModifyDTO rolDTO);
        Task Delete(int id);
    }
}
