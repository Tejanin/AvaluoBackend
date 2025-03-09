
using AvaluoAPI.Domain.Services.DesempeñoService;
using AvaluoAPI.Domain.Services.MetodoEvaluacionService;
using AvaluoAPI.Presentation.DTOs.EdificioDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesempenoController : Controller
    {
        private readonly IDesempeñoService _desempeñoService;
        private readonly PdfHelper _pdfHelper;

        public DesempenoController(IDesempeñoService DesempeñoService, PdfHelper pdfHelper)
        {
            _desempeñoService = DesempeñoService;
            _pdfHelper = pdfHelper;
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
        [HttpGet("informe/generarPDF")]
        public async Task<IActionResult> GenerarInformeDesempeñoPdf(
            [FromQuery] int? año,
            [FromQuery] string? periodo,
            [FromQuery] int? idAsignatura,
            [FromQuery] int? idSO)
        {
            var informe = await _desempeñoService.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);

            if (!informe.Any())
            {
                return BadRequest(new { mensaje = "No hay datos para generar el informe." });
            }

            // 1. Construir la ruta de almacenamiento
            string añoStr = año?.ToString() ?? "Desconocido";
            var rutaBuilder = new RutaInformeBuilder("Desempeño", añoStr);

            // 2. Construir el nombre del archivo
            string fileName = $"Informe_Desempeño_{añoStr}_{periodo ?? "Todos"}_{idAsignatura ?? 0}_{idSO ?? 0}";

            // 3. Generar y guardar el PDF
            string pdfPath = await _pdfHelper.GenerarYGuardarPdfAsync("Desempeno/InformeDesempeño", informe, rutaBuilder, fileName);

            // 4. Retornar la ruta del archivo generado
            return Ok(new { mensaje = "Informe generado y guardado exitosamente", ruta = pdfPath });
        }
    }
}
