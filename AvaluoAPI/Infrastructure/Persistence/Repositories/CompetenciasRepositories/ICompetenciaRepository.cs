using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CompetenciasRepositories
{
    public interface ICompetenciaRepository : IRepository<Competencia>
    {
        Task<IEnumerable<CompetenciaViewModel>> GetAllCompetencias();
        Task<CompetenciaViewModel> GetCompetenciaById(int id);
    }
}
