using AvaluoAPI.Domain.Services.DashboardService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }


        [HttpGet("studentOutcomes")]
        public async Task<IActionResult> GetSOTable()
        {
            return Ok(await _dashboardService.MostrarResumenSODashboardCoordinador());
        }

        [HttpGet("evaluaciones")]
        public async Task<IActionResult> GetEvaluaciones(int? recordsPerPage, int? page)
        {
            return Ok(await _dashboardService.MostrarListaEvaluacionesDeCarrera(recordsPerPage,page));
        }
    }
}
