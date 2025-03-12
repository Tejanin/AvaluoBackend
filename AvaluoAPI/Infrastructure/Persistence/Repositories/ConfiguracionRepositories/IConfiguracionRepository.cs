using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels.CofiguracionViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ConfiguracionRepositories
{
    public interface IConfiguracionRepository : IRepository<ConfiguracionEvaluaciones>
    {
        Task<ConfiguracionViewModel> GetConfiguracionById(int id);
        Task<PaginatedResult<ConfiguracionViewModel>> GetConfiguraciones(string? descripcion, DateTime? fechaInicio, DateTime? fechaCierre, int? idEstado, int? page, int? recordsPerPage);
        Task<FechaConfiguracionViewModel> GetFechaConfiguracion();
    }
}
