using AvaluoAPI.Domain.Services.AulaService;
using AvaluoAPI.Domain.Services.MetodoEvaluacionService;
using AvaluoAPI.Presentation.DTOs.AulaDTOs;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AulaController : ControllerBase
    {
        private readonly IAulaService _aulaService;

        public AulaController(IAulaService aulaService)
        {
            _aulaService = aulaService;
        }

        // GET: api/Aula
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AulaViewModel>>> Get(
            [FromQuery] string? descripcion,
            [FromQuery] int? idEdificio,
            [FromQuery] int? idEstado
            )
        {
            var aulas = await _aulaService.GetAll(descripcion, idEdificio, idEstado);
            return Ok(new { mensaje = "Aulas obtenidas exitosamente.", data = aulas });
        }

        // GET: api/Aula/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AulaViewModel>> Get(int id)
        {
            var aula = await _aulaService.GetById(id);
            return Ok(new { mensaje = "Aula encontrada.", data = aula });
        }

        // POST: api/Aula
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AulaDTO aulaDTO)
        {
            await _aulaService.Register(aulaDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Aula creada exitosamente." });
        }

        // PUT: api/Aula/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AulaDTO aulaDTO)
        {
            await _aulaService.Update(id, aulaDTO);
            return Ok(new { mensaje = "El aula ha sido actualizada con éxito." });
        }

        // DELETE: api/Aula/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _aulaService.Delete(id);
            return Ok(new { mensaje = "El aula ha sido eliminada con éxito." });
        }
    }
}
