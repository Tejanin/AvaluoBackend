﻿using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.DashboardsViewModels.AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories
{
    public interface IRubricaRepository: IRepository<Rubrica>
    {

        Task<PaginatedResult<RubricaViewModel>> GetRubricasFiltered(
            int? idSO = null,
            List<int>? carrerasIds = null,
            List<int>? estadosIds = null,
            int? idAsignatura = null,
            int? page = null,
            int? recordsPerPage = null);

        Task<List<ReporteSOViewModel>> ObtenerTableroDeSOPorArea(int area, int año, string periodo);
        Task<List<int>> ObtenerIdAsignaturasPorEstadoAsync(int idEstado);
        Task<List<SeccionRubricasViewModel>> GetProfesorSeccionesWithRubricas(int profesor, int activo, int activoSinEntregar);
        Task<List<ReporteSOViewModel>> ObtenerTableroDeSO(int idCarrera, int año, string periodo);
    }

}
