using AvaluoAPI.Domain.Services.EstadoService;
using AvaluoAPI.Presentation.DTOs.EstadoDTOs;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadoService _estadoService;

        public EstadosController(IEstadoService estadoService)
        {
            _estadoService = estadoService;
        }

        // GET: api/Estados?tabla=Competencia&descripcion=Activa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoViewModel>>> Get(
            [FromQuery] string? tabla = null,
            [FromQuery] string? descripcion = null)
        {
            var estados = await _estadoService.GetAll(tabla, descripcion);
            return Ok(new { mensaje = "Estados obtenidos exitosamente.", data = estados });
        }

        // GET: api/Estados/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoViewModel>> Get(int id)
        {
            var estado = await _estadoService.GetById(id);
            return Ok(new { mensaje = "Estado encontrado.", data = estado });
        }

        // POST: api/Estados
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EstadoDTO estadoDTO)
        {
            await _estadoService.Register(estadoDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Estado creado exitosamente." });
        }

        // PUT: api/Estados/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EstadoDTO estadoDTO)
        {
            await _estadoService.Update(id, estadoDTO);
            return Ok(new { mensaje = "El estado ha sido actualizado con éxito." });
        }

        // DELETE: api/Estados/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _estadoService.Delete(id);
            return Ok(new { mensaje = "El estado ha sido eliminado con éxito." });
        }
    }
}
