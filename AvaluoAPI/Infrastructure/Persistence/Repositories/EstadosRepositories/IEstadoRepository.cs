using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories
{
    public interface IEstadoRepository : IRepository<Estado>
    {
        Task<Estado> GetEstadoByTablaName(string idtabla, string nombre);
        
    }
}
