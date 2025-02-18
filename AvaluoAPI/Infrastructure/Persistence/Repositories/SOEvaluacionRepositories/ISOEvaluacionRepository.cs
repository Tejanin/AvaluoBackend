using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories
{
    public interface ISOEvaluacionRepository: IRepository<SOEvaluacion>
    {
        Task<IEnumerable<SOEvaluacion>> GetSOEvaluaciones();
    }
}
