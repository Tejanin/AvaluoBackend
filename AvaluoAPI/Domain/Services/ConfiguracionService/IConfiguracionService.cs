using AvaluoAPI.Presentation.DTOs.ConfiguracionDTOs;
using AvaluoAPI.Presentation.DTOs.EdificioDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.CofiguracionViewModels;

namespace AvaluoAPI.Domain.Services.ConfiguracionService
{
    public interface IConfiguracionService
    {
        Task<ConfiguracionViewModel> GetById(int id);
        Task<PaginatedResult<ConfiguracionViewModel>> GetAll(string? descripcion, DateTime? fechaInicio, DateTime? fechaCierre, int? idEstado, int? page, int? recordsPerPage);
        Task<FechaConfiguracionViewModel> GetFechas();
        Task Register(ConfiguracionDTO configuracionDTO);
        Task Update(int id, ConfiguracionDTO configuracionDTO);
    }
}
