using AvaluoAPI.Domain.Services.TipoCompetenciaService;
using AvaluoAPI.Presentation.DTOs;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
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

        // GET: api/TiposCompetencias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoCompetenciaViewModel>>> Get()
        {
            try
            {
                var tipos = await _tipoCompetenciaService.GetAll();
                if (tipos == null || !tipos.Any())
                    return NotFound(new { mensaje = "No hay tipos de competencias disponibles." });

                return Ok(new { mensaje = "Tipos de competencias obtenidos exitosamente.", data = tipos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // GET: api/TiposCompetencias/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoCompetenciaViewModel>> Get(int id)
        {
            try
            {
                var tipo = await _tipoCompetenciaService.GetById(id);
                if (tipo == null)
                    return NotFound(new { mensaje = "Tipo de competencia no encontrado." });

                return Ok(new { mensaje = "Tipo de competencia encontrado.", data = tipo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // POST: api/TiposCompetencias
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            if (tipoCompetenciaDTO == null || string.IsNullOrWhiteSpace(tipoCompetenciaDTO.Nombre))
                return BadRequest(new { mensaje = "El nombre del tipo de competencia es requerido." });

            try
            {
                await _tipoCompetenciaService.Register(tipoCompetenciaDTO);
                return CreatedAtAction(nameof(Get), new { mensaje = "Tipo de competencia creado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // PUT: api/TiposCompetencias/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            if (tipoCompetenciaDTO == null || string.IsNullOrWhiteSpace(tipoCompetenciaDTO.Nombre))
                return BadRequest(new { mensaje = "El nombre del tipo de competencia es requerido." });

            try
            {
                await _tipoCompetenciaService.Update(id, tipoCompetenciaDTO);
                return Ok(new { mensaje = "El tipo de competencia ha sido actualizado con éxito." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensaje = "Tipo de competencia no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // DELETE: api/TiposCompetencias/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tipoCompetenciaService.Delete(id);
                return Ok(new { mensaje = "El tipo de competencia ha sido eliminado con éxito." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensaje = "Tipo de competencia no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
