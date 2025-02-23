using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.EntitiesResult;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ProfesorCarreraRepositories
{
    public interface IProfesorCarreraRepository: IRepository<ProfesorCarrera>
    {
        Task<ProfesorCarrerasResult> GetProfesorWithCarreras(int profesorId);
    }
}
