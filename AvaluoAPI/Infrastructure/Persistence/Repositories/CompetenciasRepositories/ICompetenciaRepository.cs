using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories
{
    public interface ICompetenciaRepository : IRepository<Competencia>
    {
        Task<CompetenciaViewModel> GetCompetenciaById(int id);
        Task<PaginatedResult<CompetenciaViewModel>> GetCompetencias(string? nombre, string? acron, string? titulo, int? idTipo, int? idEstado, int? page, int? recordsPerPage);
        Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetMapaCompetencias(int idCarrera, int? idTipoCompetencia);
        Task<bool> UpdateEstadoMapaCompetencia(int idAsignatura, int idCompetencia, int idNuevoEstado); 
    }
}
