using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.RubricasService
{
    public interface IRubricaService
    {
        Task InsertRubricas();
        Task CompleteRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile>? evidenciasExtras);
        Task DesactivateRubricas();
        Task<IEnumerable<RubricaViewModel>> GetAllRubricas();
        Task EditRubricas(CompleteRubricaDTO rubricaDTO, List<IFormFile> evidenciasExtras);


    }
}
