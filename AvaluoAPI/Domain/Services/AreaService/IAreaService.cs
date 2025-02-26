using AvaluoAPI.Presentation.DTOs.AreaDTOs;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.AreaService
{
    public interface IAreaService
    {
        Task<AreaViewModel> GetById(int id);
        Task<PaginatedResult<AreaViewModel>> GetAll(string? descripcion, int? idCoordinador, int? page, int? recordsPerPage);
        Task Register(AreaDTO areaDTO);
        Task Update(int id, AreaDTO areaDTO);
        Task Delete(int id);
    }
}
