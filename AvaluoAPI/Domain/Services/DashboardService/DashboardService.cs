using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Presentation.ViewModels.DashboardsViewModels.AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities.JWT;

namespace AvaluoAPI.Domain.Services.DashboardService
{
    public interface IDashboardService
    {
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
    }
}
