using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AulaRepositories
{
    public interface IAulaRepository: IRepository<Aula>
    {
        Task<AulaViewModel> GetAulasById(int id);
        Task<IEnumerable<AulaViewModel>> GetAulas(string? descripcion, int? idEdificio, int? idEstado);
    }
}
