using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.HistorialIncumplimientoRepositories
{
    public interface IHistorialIncumplimientoRepository: IRepository<HistorialIncumplimiento>
    {
        Task InsertIncumplimientos(IEnumerable<Rubrica> rubricas);
        Task<PaginatedResult<HistorialIncumplimientoViewModel>> GetHistorialIncumplimientos(int? idUsuario, string? descripcion, DateTime? desde, DateTime? hasta, int? page, int? recordsPerPage);
    }
}
