using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;

namespace AvaluoAPI.Domain.Services.RubricasService
{
    public interface IRubricaService
    {
        Task InsertRubricas();
        Task CompleteRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile>? evidenciasExtras);
        Task DesactivateRubricas();
        Task<IEnumerable<RubricaViewModel>> GetAllRubricas(int? idSO = null, List<int>? carrerasIds = null, int? idEstado = null, int? idAsignatura = null);
        Task EditRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile> evidenciasExtras);
        Task<(DateTime inicio, DateTime cierre)> GetFechasCriticas();
        Task<IEnumerable<RubricaViewModel>> GetRubricasBySupervisor();

    }
}
