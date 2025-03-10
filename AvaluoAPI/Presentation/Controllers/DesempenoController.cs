using AvaluoAPI.Domain.Services.DesempeñoService;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesempenoController : Controller
    {
        private readonly IDesempeñoService _desempeñoService;

        public DesempenoController(IDesempeñoService desempeñoService)
        {
            _desempeñoService = desempeñoService;
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

            return View("InformeDesempeño", informe);
        }

        [HttpGet("informe/generarPDF")]
        public async Task<IActionResult> GenerarInformeDesempeñoPdf(
            [FromQuery] int? año,
            [FromQuery] string? periodo,
            [FromQuery] int? idAsignatura,
            [FromQuery] int? idSO)
        {
            var informe = await _desempeñoService.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);
            string pdfPath = await _desempeñoService.GenerarYGuardarPdfInforme(informe, año, periodo, idAsignatura, idSO);

            return Ok(new { mensaje = "Informe generado y guardado exitosamente", ruta = pdfPath });
        }
    }
}