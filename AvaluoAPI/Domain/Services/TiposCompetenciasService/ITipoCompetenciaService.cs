using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.TipoCompetenciaService
{
    public interface ITipoCompetenciaService
    {
        Task<TipoCompetenciaViewModel> GetById(int id); // Obtiene un tipo de competencia por su ID

        Task<IEnumerable<TipoCompetenciaViewModel>> GetAll(); // Obtiene todos los tipos de competencias

        Task Register(TipoCompetenciaDTO tipoCompetenciaDTO); // Registra un nuevo tipo de competencia

        Task Update(int id, TipoCompetenciaDTO tipoCompetenciaDTO); // Actualiza un tipo de competencia existente

        Task Delete(int id); // Elimina un tipo de competencia por su ID
    }
}
