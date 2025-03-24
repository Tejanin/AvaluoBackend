using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Presentation.ViewModels.DashboardsViewModels.AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using AvaluoAPI.Utilities.JWT;

namespace AvaluoAPI.Domain.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<PaginatedResult<RubricaViewModel>> MostrarListaEvaluacionesDeCarrera(int? recordsPerPage, int? page);
        Task<List<ReporteSOViewModel>> MostrarResumenSODashboardCoordinador();
    }
    public class DashboardService: IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        public DashboardService(IUnitOfWork unitOfWork, IJwtService jwtService) 
        { 
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ReporteSOViewModel>> MostrarResumenSODashboardCoordinador()
        {
            var id = _jwtService.GetClaimValue("Id"); 
            (int tri, int año) = PeriodoExtensions.ObtenerTrimestreActual();
            // validar si es coordinador con los permisos
            var area = await _unitOfWork.Areas.FindAsync(a => a.IdCoordinador == int.Parse(id!));
            var carrera = await _unitOfWork.Carreras.FindAsync(c => c.IdCoordinadorCarrera == int.Parse(id!));

            if (area != null) return await _unitOfWork.Rubricas.ObtenerTableroDeSOPorArea(area.Id, año, tri.ToString());
            if (carrera != null) return await _unitOfWork.Rubricas.ObtenerTableroDeSO(carrera.Id, año, tri.ToString());

            throw new UnauthorizedAccessException("No tienes acceso al tablero");
        }

        public async Task<PaginatedResult<RubricaViewModel>> MostrarListaEvaluacionesDeCarrera(int? recordsPerPage, int? page)    
        {
            List<int> idsCarreras = new();
            int id = int.Parse(_jwtService.GetClaimValue("Id")!);
            var area = await _unitOfWork.Areas.FindAsync(a => a.IdCoordinador == id);
            var carrera = await _unitOfWork.Carreras.FindAsync(c => c.IdCoordinadorCarrera == id);

            if (area != null)
            {
                var carreras = await _unitOfWork.Carreras.FindAllAsync(c => c.IdArea == area.Id);
                idsCarreras = carreras.Select(c => c.Id).ToList();
            }
            

            var activaSinEntrega = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y sin entregar");
            var activaEntregada = await _unitOfWork.Estados.GetEstadoByTablaName("Rubrica", "Activa y entregada");
            List<int> estados = new List<int> { activaSinEntrega.Id, activaEntregada.Id };

            if (area != null) return await _unitOfWork.Rubricas.GetRubricasFiltered(carrerasIds: idsCarreras, estadosIds: estados, recordsPerPage: recordsPerPage, page: page);
            if (carrera != null) return await _unitOfWork.Rubricas.GetRubricasFiltered(carrerasIds:new List<int> { carrera.Id}, estadosIds: estados, recordsPerPage: recordsPerPage, page: page);

            throw new UnauthorizedAccessException("No tienes acceso al tablero");




        }
    }
}
