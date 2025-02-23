using AvaluoAPI.Presentation.DTOs.AsignaturaDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.AsignaturaService
{
    public interface IAsignaturaService
    {
        Task<PaginatedResult<AsignaturaViewModel>> GetAll(string? codigo, string? nombre, int? idEstado, int? idArea, int? page, int? recordsPerPage);
        Task<AsignaturaViewModel> GetById(int id);
        Task Register(AsignaturaDTO asignaturaDTO);
        Task Update(int id, AsignaturaModifyDTO asignaturaDTO);
        Task Delete(int id);
    }

}
