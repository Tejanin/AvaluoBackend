using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.RubricasService
{
    public interface IRubricaService
    {
        Task<IEnumerable<AsignaturaConCompetenciasViewModel>> InsertRubricas();
    }
}
