using AvaluoAPI.Domain.Services.CarreraService;
using AvaluoAPI.Presentation.DTOs.CarreraDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrerasController : ControllerBase
    {
        private readonly ICarreraService _carreraService;

        public CarrerasController(ICarreraService carreraService)
        {
            _carreraService = carreraService;
        }

        // GET: api/Carreras?nombreCarrera=Ingeniería&idEstado=1&idArea=2&idCoordinador=5&page=1&recordsPerPage=10
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<CarreraViewModel>>> Get(
            [FromQuery] string? nombreCarrera,
            [FromQuery] int? año,
            [FromQuery] string? peos,
            [FromQuery] int? idEstado,
            [FromQuery] int? idArea,
            [FromQuery] int? idCoordinador,
            [FromQuery] int? page,
            [FromQuery] int? recordsPerPage)
        {
            var carreras = await _carreraService.GetAll(nombreCarrera, idEstado, idArea, idCoordinador,año, peos, page, recordsPerPage);
            return Ok(new { mensaje = "Carreras obtenidas exitosamente.", data = carreras });
        }

        // GET: api/Carreras/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CarreraViewModel>> Get(int id)
        {
            var carrera = await _carreraService.GetById(id);
            return Ok(new { mensaje = "Carrera encontrada.", data = carrera });
        }

        // POST: api/Carreras
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CarreraDTO carreraDTO)
        {
            await _carreraService.Register(carreraDTO);
            return CreatedAtAction(nameof(Post), new { mensaje = "Carrera creada exitosamente." });
        }

        // PUT: api/Carreras/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CarreraModifyDTO carreraDTO)
        {
            await _carreraService.Update(id, carreraDTO);
            return Ok(new { mensaje = "La carrera ha sido actualizada con éxito." });
        }

        [HttpPatch("peos/{id}")]
        public async Task<IActionResult> UpdatePEOs(int id, [FromBody] string nuevosPEOs)
        {
            if (string.IsNullOrWhiteSpace(nuevosPEOs))
                return BadRequest(new { mensaje = "Los PEOs no pueden estar vacíos." });

            await _carreraService.UpdatePEOs(id, nuevosPEOs);
            return Ok(new { mensaje = "PEOs de la carrera actualizados con éxito." });
        }

        // DELETE: api/Carreras/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _carreraService.Delete(id);
            return Ok(new { mensaje = "La carrera ha sido eliminada con éxito." });
        }

    }
}
