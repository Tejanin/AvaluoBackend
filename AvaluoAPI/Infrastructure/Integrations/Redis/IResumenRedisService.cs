using AvaluoAPI.Presentation.DTOs.RubricaDTOs;

namespace AvaluoAPI.Infrastructure.Integrations.Redis
{
    public interface IResumenRedisService
    {
        Task<bool> SaveResumenAsync(string key, ResumenDTO resumen);
        Task<bool> SaveResumenListAsync(string key, List<ResumenDTO> resumenes);
        Task<ResumenDTO> GetResumenAsync(string key);
        Task<List<ResumenDTO>> GetResumenListAsync(string key);
        Task<bool> UpdateResumenAsync(string key, ResumenDTO resumen);
        Task<bool> UpdateEstudianteCalificacionAsync(string resumenKey, int idPI, string matricula, int nuevaCalificacion);
        Task<bool> DeleteResumenAsync(string key);
    }
}
