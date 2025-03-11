using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories
{
    public interface ISOEvaluacionRepository: IRepository<SOEvaluacion>
    {
        Task<IEnumerable<SOEvaluacion>> GetSOEvaluaciones();
        Task<IEnumerable<MetodoEvaluacionViewModel>> GetMetodosEvaluacionPorSO(int idSO);
    }
}
