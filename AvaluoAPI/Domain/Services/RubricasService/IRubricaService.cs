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
        Task<PaginatedResult<RubricaViewModel>> GetAllRubricas(int? idSO = null, List<int>? carreras = null, List<int>? estado = null, int? idAsignatura = null, int? page = null, int? recordsPerPage = null);
        Task EditRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile> evidenciasExtras);
        Task<(DateTime inicio, DateTime cierre)> GetFechasCriticas();
        Task<IEnumerable<RubricaViewModel>> GetRubricasBySupervisor();
        Task<List<SeccionRubricasViewModel>> GetProfesorSecciones();

    }
}
