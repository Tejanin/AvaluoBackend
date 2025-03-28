using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;

namespace AvaluoAPI.Domain.Services.HistorialIncumplimientoService
{
    public interface IHistorialIncumplimientoService
    {
        Task<PaginatedResult<HistorialIncumplimientoViewModel>> GetAll(int? idUsuario, string? descripcion, DateTime? desde, DateTime? hasta, int? page, int? recordsPerPage);
    }
}
