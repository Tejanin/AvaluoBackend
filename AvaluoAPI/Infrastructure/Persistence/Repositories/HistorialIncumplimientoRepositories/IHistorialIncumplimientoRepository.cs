using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.HistorialIncumplimientoRepositories
{
    public interface IHistorialIncumplimientoRepository: IRepository<HistorialIncumplimiento>
    {
        Task InsertIncumplimientos(IEnumerable<Rubrica> rubricas);
    }
}
