using AvaluoAPI.Domain.Services.AreaService;
using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Presentation.DTOs.AreaDTOs;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController: ControllerBase
    {
        private readonly IAreaService _areaService;

        public AreasController(IAreaService areaService)
        {
            _areaService = areaService;
        }


        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<AreaViewModel>>> Get(
            [FromQuery] string? descripcion,
            [FromQuery] int? idCoordinador,
            [FromQuery] int? page,
            [FromQuery] int? recordsPerPage)
        {
            var areas = await _areaService.GetAll(descripcion, idCoordinador, page, recordsPerPage);
            return Ok(new { mensaje = "Areas obtenidas exitosamente.", data = areas });
        }


        // GET: api/Areas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AreaViewModel>> Get(int id)
        {
            var area = await _areaService.GetById(id);
            return Ok(new { mensaje = "Area encontrada.", data = area });
        }

        // POST: api/Areas
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AreaDTO areaDTO)
        {
            await _areaService.Register(areaDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Area creada exitosamente." });
        }

        // PUT: api/Areas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AreaDTO areaDTO)
        {
            await _areaService.Update(id, areaDTO);
            return Ok(new { mensaje = "El Area ha sido actualizada con éxito." });
        }

        // DELETE: api/Areas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _areaService.Delete(id);
            return Ok(new { mensaje = "El Area ha sido eliminada con éxito." });
        }
    }
}
