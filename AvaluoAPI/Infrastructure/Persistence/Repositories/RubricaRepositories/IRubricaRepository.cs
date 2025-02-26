using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories
{
    public interface IRubricaRepository: IRepository<Rubrica>
    {
        Task<IEnumerable<RubricaViewModel>> GetRubricasFiltered(int? idSO = null, List<int>? carrerasIds = null, int? idEstado = null, int? idAsignatura = null);
        Task<List<int>> ObtenerIdAsignaturasPorEstadoAsync(int idEstado);
        Task<List<SeccionRubricasViewModel>> GetProfesorSeccionesWithRubricas(int profesor, int activo, int activoSinEntregar);
    }

}
