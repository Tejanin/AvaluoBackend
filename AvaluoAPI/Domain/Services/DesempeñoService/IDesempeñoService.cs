using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.DesempeñoService
{
    public interface IDesempeñoService
    {
        Task<IEnumerable<InformeDesempeñoViewModel>> GenerarInformeDesempeño(int? año, string? periodo, int? idAsignatura, int? idSO = null);
    }
}
