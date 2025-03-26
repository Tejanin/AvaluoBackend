using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.InformesRepositories
{
    public interface IInformeRepository: IRepository<Informe>
    {
        Task<PaginatedResult<InformeViewModel>> GetAllInformesAsync(int? idTipo, int? idCarrera, string? nombre, int? año, char? trimestre, string? periodo, int? page, int? recordsPerPage);
        Task<InformeViewModel?> GetInformeByIdAsync(int id);
    }
}
