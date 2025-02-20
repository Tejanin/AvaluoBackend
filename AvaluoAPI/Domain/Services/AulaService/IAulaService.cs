using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.AulaDTOs;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.AulaService
{
    public interface IAulaService
    {
        Task<AulaViewModel> GetById(int id);

        Task<IEnumerable<AulaViewModel>> GetAll(string? descripcion, int? idEdificio, int? idEstado);

        Task Register(AulaDTO aulaDTO);

        Task Update(int id, AulaDTO aulaDTO);

        Task Delete(int id);
    }
}
