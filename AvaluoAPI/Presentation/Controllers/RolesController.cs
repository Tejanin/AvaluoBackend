using AvaluoAPI.Domain.Services.RolService;
using AvaluoAPI.Presentation.DTOs.RolDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolService _rolService;

        public RolesController(IRolService rolService)
        {
            _rolService = rolService;
        }

        // GET: api/Roles?descripcion=Admin&page=1&recordsPerPage=10
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<RolViewModel>>> Get(
            [FromQuery] string? descripcion,
            [FromQuery] int? page,
            [FromQuery] int? recordsPerPage)
        {
            var roles = await _rolService.GetAll(descripcion, page, recordsPerPage);
            return Ok(new { mensaje = "Roles obtenidos exitosamente.", data = roles });
        }

        // GET: api/Roles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RolViewModel>> Get(int id)
        {
            var rol = await _rolService.GetById(id);
            return Ok(new { mensaje = "Rol encontrado.", data = rol });
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RolDTO rolDTO)
        {
            await _rolService.Register(rolDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Rol creado exitosamente." });
        }

        // PUT: api/Roles/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RolModifyDTO rolDTO)
        {
            await _rolService.Update(id, rolDTO);
            return Ok(new { mensaje = "El rol ha sido actualizado con éxito." });
        }

        // DELETE: api/Roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _rolService.Delete(id);
            return Ok(new { mensaje = "El rol ha sido eliminado con éxito." });
        }
    }
}
