using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.MapaCompetenciaRepositories
{
    public interface IMapaCompetenciaRepository: IRepository<MapaCompetencias>
    {
        Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetAsignaturasConCompetencias();
    }
}
