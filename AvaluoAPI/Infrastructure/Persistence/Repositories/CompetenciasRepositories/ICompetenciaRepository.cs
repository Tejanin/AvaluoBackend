using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories
{
    public interface ICompetenciaRepository : IRepository<Competencia>
    {
        Task<CompetenciaViewModel> GetCompetenciaById(int id);
        Task<IEnumerable<CompetenciaViewModel>> GetCompetenciasByFilter(string? nombre, string? acron, string? titulo, int? idTipo, int? idEstado);
    }
}
