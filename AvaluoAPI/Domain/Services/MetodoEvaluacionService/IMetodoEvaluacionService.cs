using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.MetodoEvaluacionService
{
    public interface IMetodoEvaluacionService
    {
        Task<MetodoEvaluacionViewModel> GetById(int id);

        Task<IEnumerable<MetodoEvaluacionViewModel>> GetAll();

        Task Register(MetodoEvaluacionDTO metodoEvaluacionDTO);

        Task Update(int id, MetodoEvaluacionDTO metodoEvaluacionDTO);

        Task Delete(int id);

        Task<IEnumerable<MetodoEvaluacionViewModel>> GetMetodosEvaluacionPorSO(int idSO);
        Task RegisterSOEvaluacion(int idSO, int idMetodoEvaluacion);
        Task DeleteSOEvaluacion(int idSO, int idMetodoEvaluacion);
    }
}
