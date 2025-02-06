using AvaluoAPI.Domain.Services.TipoInformeService;
using AvaluoAPI.Presentation.DTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            var tipos = await _tipoInformeService.GetAll();
            return Ok(new { mensaje = "Tipos de informes obtenidos exitosamente.", data = tipos });
        }

        // GET: api/TipoInformes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoInformeViewModel>> Get(int id)
        {
            var tipo = await _tipoInformeService.GetById(id);
            return Ok(new { mensaje = "Tipo de informe encontrado.", data = tipo });
        }

        // POST: api/TipoInformes
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TipoInformeDTO tipoInformeDTO)
        {
            await _tipoInformeService.Register(tipoInformeDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Tipo de informe creado exitosamente." });
        }

        // PUT: api/TipoInformes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TipoInformeDTO tipoInformeDTO)
        {
            await _tipoInformeService.Update(id, tipoInformeDTO);
            return Ok(new { mensaje = "El tipo de informe ha sido actualizado con éxito." });
        }

        // DELETE: api/TipoInformes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _tipoInformeService.Delete(id);
            return Ok(new { mensaje = "El tipo de informe ha sido eliminado con éxito." });
        }
    }
}
