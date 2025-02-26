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
        public async Task<IActionResult> GetAllRubricas([FromQuery]int? so = null, [FromQuery] List<int>? carreras = null, [FromQuery] int? estado = null, [FromQuery] int? asignatura = null)
        {
            
            return Ok(new { mensaje = "Operacion exitosa", data = await _rubricaService.GetAllRubricas(so,carreras,estado,asignatura) });
        }

        [HttpGet("fechas")]

        public async Task<IActionResult> GetFechasCriticas()
        {
            (var Inicio, var Cierre) = await _rubricaService.GetFechasCriticas();
            return Ok(new { mensaje = "Operacion exitosa", data = new { inicio = Inicio, cierre = Cierre } });
        }

        [HttpGet("supervisor")]
        public async Task<IActionResult> GetRubricasBySupervisor()
        {
            await _rubricaService.GetRubricasBySupervisor();
            return Ok();
        }

        [HttpGet("secciones")]
        public async Task<IActionResult> GetSeccionesByProfesor()
        {
            return Ok(await _rubricaService.GetProfesorSecciones());
        }


    }
}
