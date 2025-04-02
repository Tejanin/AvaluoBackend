using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Domain.Services.AsignaturaService;
using AvaluoAPI.Presentation.DTOs.AsignaturaCarreraDTOs;
using AvaluoAPI.Presentation.DTOs.AsignaturaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignaturasController : ControllerBase
    {
        private readonly IAsignaturaService _asignaturaService;

        public AsignaturasController(IAsignaturaService asignaturaService)
        {
            _asignaturaService = asignaturaService;
        }

        // GET: api/Asignaturas?codigo=XYZ&nombre=Matematica&idEstado=1&idArea=2&page=1&recordsPerPage=10
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<AsignaturaViewModel>>> Get(
            [FromQuery] string? codigo = null,
            [FromQuery] string? nombre = null,
            [FromQuery] int? idEstado = null,
            [FromQuery] int? idArea = null,
            [FromQuery] int? page = null,
            [FromQuery] int? recordsPerPage = null)
        {
            var asignaturas = await _asignaturaService.GetAll(codigo, nombre, idEstado, idArea, page, recordsPerPage);
            return Ok(new { mensaje = "Asignaturas obtenidas exitosamente.", data = asignaturas });
        }

        [HttpGet("carrera/{idCarrera}")]
        public async Task<ActionResult<IEnumerable<AsignaturaViewModel>>> GetByCareer(
        int idCarrera, [FromQuery] int? page, int? recordsPerPage)
        {
            var asignaturasCarreras = await _asignaturaService.GetSubjectByCareer(idCarrera, page, recordsPerPage);
            return Ok(new { mensaje = "Asignaturas por carrera obtenidas exitosamente.", data = asignaturasCarreras });
        }

        // POST: api/Asignaturas/carrera
        [HttpPost("carrera")]
        public async Task<IActionResult> RegisterSubjectByCareer([FromBody] AsignaturaCarreraDTO asignaturaCarreraDTO)
        {
            await _asignaturaService.RegisterSubjectByCareer(asignaturaCarreraDTO);
            return Ok(new { mensaje = "Asignatura asociada a la carrera exitosamente." });
        }

        // DELETE: api/Asignaturas/carrera
        [HttpDelete("carrera")]
        public async Task<IActionResult> DeleteSubjectByCareer([FromBody] AsignaturaCarreraDTO asignaturaCarreraDTO)
        {
            await _asignaturaService.DeleteGetSubjectByCareer(asignaturaCarreraDTO);
            return Ok(new { mensaje = "Asignatura eliminada de la carrera exitosamente." });
        }

        [HttpPut("change-pa/{id}")]
        public async Task<ActionResult> ChangePa(int id, IFormFile file)
        {
            await _asignaturaService.UpdateDocument(id, file, "Programa");
            return Accepted(new { message = "Programa de asignatura actualizado exitosamente" });
        }

        [HttpPut("change-syllabus/{id}")]
        public async Task<ActionResult> ChangeSyllabus(int id, IFormFile file)
        {
            await _asignaturaService.UpdateDocument(id, file, "Syllabus");
            return Accepted(new { message = "Syllabus actualizado exitosamente" });
        }

        // GET: api/Asignaturas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AsignaturaViewModel>> Get(int id)
        {
            var asignatura = await _asignaturaService.GetById(id);
            return Ok(new { mensaje = "Asignatura encontrada.", data = asignatura });
        }

        // POST: api/Asignaturas
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AsignaturaDTO asignaturaDTO)
        {
            await _asignaturaService.Register(asignaturaDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Asignatura creada exitosamente." });
        }

        // PUT: api/Asignaturas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AsignaturaModifyDTO asignaturaDTO)
        {
            await _asignaturaService.Update(id, asignaturaDTO);
            return Ok(new { mensaje = "La asignatura ha sido actualizada con éxito." });
        }

        // DELETE: api/Asignaturas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _asignaturaService.Delete(id);
            return Ok(new { mensaje = "La asignatura ha sido eliminada con éxito." });
        }
    }
}
