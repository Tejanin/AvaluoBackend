using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetenciasController : ControllerBase
    {
        private readonly ICompetenciaService _competenciaService;

        public CompetenciasController(ICompetenciaService competenciaService)
        {
            _competenciaService = competenciaService;
        }

        // GET: api/Competencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompetenciaViewModel>>> Get()
        {
            var competencias = await _competenciaService.GetAll();
            return Ok(new { mensaje = "Competencias obtenidas exitosamente.", data = competencias });
        }

        // GET: api/Competencias/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CompetenciaViewModel>> Get(int id)
        {
            var competencia = await _competenciaService.GetById(id);
            return Ok(new { mensaje = "Competencia encontrada.", data = competencia });
        }

        // POST: api/Competencias
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CompetenciaDTO competenciaDTO)
        {
            await _competenciaService.Register(competenciaDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Competencia creada exitosamente." });
        }

        // PUT: api/Competencias/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CompetenciaDTO competenciaDTO)
        {
            await _competenciaService.Update(id, competenciaDTO);
            return Ok(new { mensaje = "La competencia ha sido actualizada con éxito." });
        }

        // DELETE: api/Competencias/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _competenciaService.Delete(id);
            return Ok(new { mensaje = "La competencia ha sido eliminada con éxito." });
        }
    }
}
