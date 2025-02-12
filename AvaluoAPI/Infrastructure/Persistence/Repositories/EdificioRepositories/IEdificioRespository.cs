using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories
{
    public interface IEdificioRespository: IRepository<Edificio>
    {
        Task<IEnumerable<EdificioViewModel>> GetAllEdificios();
        Task<EdificioViewModel> GetEdificioById(int id);
    }
}
