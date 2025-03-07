
using AvaluoAPI.Domain.Services.DesempeñoService;
using AvaluoAPI.Domain.Services.MetodoEvaluacionService;
using AvaluoAPI.Presentation.DTOs.EdificioDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesempenoController : Controller
    {
        private readonly IDesempeñoService _desempeñoService;

        public DesempenoController(IDesempeñoService DesempeñoService)
        {
            _desempeñoService = DesempeñoService;
        }

        [HttpGet("informe")]
        public async Task<IActionResult> GenerarInformeDesempeño(
            [FromQuery] int? año,
            [FromQuery] string? periodo,
            [FromQuery] int? idAsignatura,
            [FromQuery] int? idSO)
        {
            var informe = await _desempeñoService.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);
            return Ok(new { mensaje = "Informe generado exitosamente", data = informe });
        }

        [HttpGet("informe/view")]
        public async Task<IActionResult> GenerarInformeDesempeñoHtml(
            [FromQuery] int? año,
            [FromQuery] string? periodo,
            [FromQuery] int? idAsignatura,
            [FromQuery] int? idSO)
        {
            var informe = await _desempeñoService.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);

            if (!informe.Any())
            {
                return Content("<h1>No hay datos para el informe solicitado.</h1>", "text/html");
            }

            return View("InformeDesempeño", informe);
        }

    }
}
