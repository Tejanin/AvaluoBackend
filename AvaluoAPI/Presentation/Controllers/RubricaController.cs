using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Domain.Services.RubricasService;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RubricaController : ControllerBase
    {
        private readonly IRubricaService _rubricaService;
        public RubricaController(IRubricaService rubricaService) 
        {
            _rubricaService = rubricaService;   
        }

        [HttpPost]
        public async Task<IActionResult> CompleteRubricas(CompleteRubricaFormDTO rubricaDTO)
        {
            await _rubricaService.CompleteRubricas(rubricaDTO.GetRubricaDTO(),rubricaDTO.Evidencias);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditRubricas(CompleteRubricaFormDTO rubricaDTO)
        {
            await _rubricaService.EditRubricas(rubricaDTO.GetRubricaDTO(), rubricaDTO.Evidencias);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRubricas([FromQuery] int? so = null, [FromQuery] List<int>? carreras = null, [FromQuery] List<int>? estados = null, [FromQuery] int? asignatura = null, [FromQuery] int? page = null, [FromQuery] int? recordsPerPage = null)
        {
            return Ok(new { mensaje = "Operación exitosa", data = await _rubricaService.GetAllRubricas(so, carreras, estados, asignatura, page, recordsPerPage) });
        }

        [HttpGet("fechas")]

        public async Task<IActionResult> GetFechasCriticas()
        {
            (var Inicio, var Cierre) = await _rubricaService.GetFechasCriticas();
            return Ok(new { mensaje = "Operacion exitosa", data = new { inicio = Inicio, cierre = Cierre } });
        }

        [HttpGet("supervisor")]
        public async Task<IActionResult> GetRubricasBySupervisor(int? page = null, [FromQuery] int? recordsPerPage = null)
        {
            
            return Ok(new {data = await _rubricaService.GetRubricasBySupervisor(page, recordsPerPage) , message = "Operacion Exitosa"});
        }

        [HttpGet("secciones")]
        public async Task<IActionResult> GetSeccionesByProfesor()
        {
            return Ok(new { data = await _rubricaService.GetProfesorSecciones(), mensaje = "Operacion Exitosa" });
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InseertRubricas()
        {
            await _rubricaService.InsertRubricas();
            return Ok();
        }

        [HttpPut("desactivate")]
        public async Task<IActionResult> DesactivateRubricas()
        {
            await _rubricaService.DesactivateRubricas();
            return Ok();
        }
    }
}
