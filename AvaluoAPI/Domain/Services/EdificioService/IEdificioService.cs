using AvaluoAPI.Presentation.DTOs.EdificioDTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.EdificioService
{
    public interface IEdificioService
    {
        Task<EdificioViewModel> GetById(int id); // Obtiene un tipo de informe por su ID
        Task<IEnumerable<EdificioViewModel>> GetAll(); // Obtiene todos los tipos de informes

        Task Register(EdificioDTO edificioDTO); // Registra un nuevo tipo de informe

        Task Update(int id, EdificioDTO edificioDTO); // Actualiza un tipo de informe existente

        Task Delete(int id); // Elimina un tipo de informe por su ID
    }
}
