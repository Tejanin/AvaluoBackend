using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;

namespace AvaluoAPI.Domain.Services.MetodoEvaluacionService
{
    public interface IMetodoEvaluacionService
    {
        Task<MetodoEvaluacion> GetById(int id);

        Task<IEnumerable<MetodoEvaluacion>> GetAll();

        Task Register(MetodoEvaluacionDTO metodoEvaluacionDTO);

        Task Update(int id, MetodoEvaluacionDTO metodoEvaluacionDTO);

        Task Delete(int id);
    }
}
