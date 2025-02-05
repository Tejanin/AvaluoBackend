using AvaluoAPI.Domain.Services.TipoInformeService;
using AvaluoAPI.Presentation.DTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoInformesController : ControllerBase
    {
        private readonly ITipoInformeService _tipoInformeService;

        public TipoInformesController(ITipoInformeService tipoInformeService)
        {
            _tipoInformeService = tipoInformeService;
        }

        // GET: api/TipoInformes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoInformeViewModel>>> Get()
        {
            try
            {
                var tipos = await _tipoInformeService.GetAll();
                if (tipos == null || !tipos.Any())
                    return NotFound(new { mensaje = "No hay tipos de informes disponibles." });

                return Ok(new { mensaje = "Tipos de informes obtenidos exitosamente.", data = tipos });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // GET: api/TipoInformes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoInformeViewModel>> Get(int id)
        {
            try
            {
                var tipo = await _tipoInformeService.GetById(id);
                if (tipo == null)
                    return NotFound(new { mensaje = "Tipo de informe no encontrado." });

                return Ok(new { mensaje = "Tipo de informe encontrado.", data = tipo });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // POST: api/TipoInformes
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoInformeDTO tipoInformeDTO)
        {
            if (tipoInformeDTO == null || string.IsNullOrWhiteSpace(tipoInformeDTO.Descripcion))
                return BadRequest(new { mensaje = "La descripción del tipo de informe es requerida." });

            try
            {
                await _tipoInformeService.Register(tipoInformeDTO);
                return CreatedAtAction(nameof(Get), new { mensaje = "Tipo de informe creado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // PUT: api/TipoInformes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TipoInformeDTO tipoInformeDTO)
        {
            if (tipoInformeDTO == null || string.IsNullOrWhiteSpace(tipoInformeDTO.Descripcion))
                return BadRequest(new { mensaje = "La descripción del tipo de informe es requerida." });

            try
            {
                await _tipoInformeService.Update(id, tipoInformeDTO);
                return Ok(new { mensaje = "El tipo de informe ha sido actualizado con éxito." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensaje = "Tipo de informe no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        // DELETE: api/TipoInformes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tipoInformeService.Delete(id);
                return Ok(new { mensaje = "El tipo de informe ha sido eliminado con éxito." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { mensaje = "Tipo de informe no encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }
    }
}
