using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;
using NuGet.Protocol.Core.Types;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.IDesempeñoRepositories
{
    public interface IDesempeñoRepository : IRepository<Desempeno>
    {
        Task InsertDesempeños(List<int> asignaturas, int año, string periodo, int estado);
        Task<IEnumerable<InformeDesempeñoViewModel>> GenerarInformeDesempeño(int? año = null, string? periodo = null, int? idAsignatura = null, int? idSO = null);
        Task<List<int>> ObtenerIdSOPorAsignaturasAsync(int año, string periodo, int idAsignatura);
    }
}
