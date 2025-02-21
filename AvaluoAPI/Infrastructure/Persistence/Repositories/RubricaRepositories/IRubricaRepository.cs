using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories
{
    public interface IRubricaRepository: IRepository<Rubrica>
    {
        Task<IEnumerable<RubricaViewModel>> GetAllRubricas();
    }

}
