using AvaluoAPI.Presentation.DTOs.EstadoDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.EstadoService
{
    public interface IEstadoService
    {
        Task<EstadoViewModel> GetById(int id); // Obtiene un estado por su ID

        Task<IEnumerable<EstadoViewModel>> GetAll(string? idTabla, string? descripcion); // Obtiene todos los estados y filtros

        Task Register(EstadoDTO estadoDTO); // Registra un nuevo estado

        Task Update(int id, EstadoDTO estadoDTO); // Actualiza un estado existente

        Task Delete(int id); // Elimina un estado por su ID
    }
}
