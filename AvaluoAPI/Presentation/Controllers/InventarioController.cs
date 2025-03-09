using AvaluoAPI.Domain.Services.InventarioService;
using AvaluoAPI.Presentation.DTOs.InventarioDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioService _inventarioService;

        public InventarioController(IInventarioService inventarioService)
        {
            _inventarioService = inventarioService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<InventarioViewModel>>> Get(
            [FromQuery] string? descripcion,
            [FromQuery] int? page,
            [FromQuery] int? recordsPerPage)
        {
            var inventario = await _inventarioService.GetAll(descripcion, page, recordsPerPage);
            return Ok(new { mensaje = "Inventario obtenido exitosamente.", data = inventario });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventarioViewModel>> Get(int id)
        {
            var inventario = await _inventarioService.GetById(id);
            return Ok(new { mensaje = "Inventario encontrado.", data = inventario });
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] InventarioDTO inventarioDTO)
        {
            await _inventarioService.Register(inventarioDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Inventario creado exitosamente." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] InventarioDTO inventarioDTO)
        {
            await _inventarioService.Update(id, inventarioDTO);
            return Ok(new { mensaje = "Inventario actualizado con éxito." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _inventarioService.Delete(id);
            return Ok(new { mensaje = "Inventario eliminado con éxito." });
        }

        [HttpGet("ubicacion")]
        public async Task<ActionResult<List<InventarioViewModel>>> GetByLocation(
        [FromQuery] int? edificioId,
        [FromQuery] int? aulaId)
        {
            var inventario = await _inventarioService.GetByLocation(edificioId, aulaId);
            return Ok(new { mensaje = "Inventario obtenido con éxito.", data = inventario });
        }

    }
}
