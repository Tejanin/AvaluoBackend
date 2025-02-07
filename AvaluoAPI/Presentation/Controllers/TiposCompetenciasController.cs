using AvaluoAPI.Domain.Services.TipoCompetenciaService;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoCompetenciasController : ControllerBase
    {
        private readonly ITipoCompetenciaService _tipoCompetenciaService;

        public TipoCompetenciasController(ITipoCompetenciaService tipoCompetenciaService)
        {
            _tipoCompetenciaService = tipoCompetenciaService;
        }

        // GET: api/TipoCompetencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCompetenciaViewModel>>> Get()
        {
            var tipos = await _tipoCompetenciaService.GetAll();
            return Ok(new { mensaje = "Tipos de competencias obtenidos exitosamente.", data = tipos });
        }

        // GET: api/TipoCompetencias/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCompetenciaViewModel>> Get(int id)
        {
            var tipo = await _tipoCompetenciaService.GetById(id);
            return Ok(new { mensaje = "Tipo de competencia encontrado.", data = tipo });
        }

        // POST: api/TipoCompetencias
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            await _tipoCompetenciaService.Register(tipoCompetenciaDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Tipo de competencia creado exitosamente." });
        }

        // PUT: api/TipoCompetencias/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            await _tipoCompetenciaService.Update(id, tipoCompetenciaDTO);
            return Ok(new { mensaje = "El tipo de competencia ha sido actualizado con éxito." });
        }

        // DELETE: api/TipoCompetencias/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tipoCompetenciaService.Delete(id);
            return Ok(new { mensaje = "El tipo de competencia ha sido eliminado con éxito." });
        }
    }
}
