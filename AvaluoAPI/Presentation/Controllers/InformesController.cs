using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Domain.Services.DesempeñoService;
using AvaluoAPI.Domain.Services.InformeService;
using AvaluoAPI.Presentation.DTOs.InformeDTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models.TermStore;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformesController : Controller
    {
        private readonly IInformeService _informeService;
        private readonly IDesempeñoService _desempeñoService;
        public InformesController(IDesempeñoService desempeñoService, IInformeService informeService)
        {
            _desempeñoService = desempeñoService;
            _informeService = informeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInformes([FromQuery] int? año, [FromQuery] char? trimestre, [FromQuery] string? periodo, [FromQuery] int? idTipo, [FromQuery] int? idCarrera, [FromQuery] string? nombre, [FromQuery] int? page, [FromQuery] int? recordsPerPage)
        {
            var informes = await _informeService.GetAll(idTipo, idCarrera, nombre, año, trimestre, periodo, page, recordsPerPage);
            return Ok(new { mensaje = "Informes obtenidos exitosamente.", data = informes});
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetInforme(int id)
        {
            var informe = await _informeService.GetById(id);

            return Ok(new
            {
                mensaje = "Informe encontrado exitosamente.",
                data = informe
            });
        }

        [HttpGet("informe/vistaPreviaJSON")]
        public async Task<IActionResult> GenerarInformeDesempeño(
            [FromQuery] int? año,
            [FromQuery] string? periodo,
            [FromQuery] int? idAsignatura,
            [FromQuery] int? idSO)
        {
            var informe = await _desempeñoService.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);
            return Ok(new { mensaje = "Informe generado exitosamente", data = informe });
        }

        [HttpGet("informe/vistaPreviaHTML")]
        public async Task<IActionResult> GenerarInformeDesempeñoHtml(
            [FromQuery] int? año,
            [FromQuery] string? periodo,
            [FromQuery] int? idAsignatura,
            [FromQuery] int? idSO)
        {
            var informe = await _desempeñoService.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);

            return View("InformeDesempeño", informe);
        }

        [HttpGet("informe/generarInforme/{anio?}/{periodo?}/{idAsignatura?}/{idSO?}")]
        public async Task<IActionResult> GenerarInformeDesempeñoPdf(
            [FromRoute] int? anio,
            [FromRoute] string? periodo,
            [FromRoute] int? idAsignatura,
            [FromRoute] int? idSO)
        {
            var informes = await _desempeñoService.GenerarInformeDesempeño(anio, periodo, idAsignatura, idSO);
            string pdfPath = await _desempeñoService.GenerarYGuardarPdfInforme(informes, anio, periodo, idAsignatura, idSO);
            pdfPath = pdfPath.Replace("\\", "/");

            foreach (var informe in informes)
            {
                await _informeService.RegistrarInformeGenerado(informe, pdfPath);
            }

            return Ok(new { mensaje = "Informe generado y guardado exitosamente", ruta = pdfPath });
        }


    }
}
