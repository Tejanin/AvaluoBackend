using AvaluoAPI.Presentation.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.DesempeñoService
{
    public interface IDesempeñoService
    {
        Task<IEnumerable<InformeDesempeñoViewModel>> GenerarInformeDesempeño(int? año, string? periodo, int? idAsignatura, int? idSO = null);
        Task<string> GenerarYGuardarPdfInforme(IEnumerable<InformeDesempeñoViewModel> informe, int? año, string? periodo, int? idAsignatura, int? idSO);
    }
}