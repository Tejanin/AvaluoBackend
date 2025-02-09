using AvaluoAPI.Domain.Services.EdificioService;
using AvaluoAPI.Domain.Services.TipoInformeService;
using AvaluoAPI.Presentation.DTOs.EdificioDTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdificioController : ControllerBase
    {
        private readonly IEdificioService _edificioService;

        public EdificioController(IEdificioService edificioService)
        {
            _edificioService = edificioService;
        }

        // GET: api/Edificios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoInformeViewModel>>> Get()
        {
            var edificios = await _edificioService.GetAll();
            return Ok(new { mensaje = "Edificios obtenidos exitosamente.", data = edificios });
        }

        // GET: api/Edificios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EdificioViewModel>> Get(int id)
        {
            var edificio = await _edificioService.GetById(id);
            return Ok(new { mensaje = "Edificio encontrado.", data = edificio });
        }

        // POST: api/Edificios
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EdificioDTO edificioDTO)
        {
            await _edificioService.Register(edificioDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Edificio creado exitosamente." });
        }

        // PUT: api/Edificios/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EdificioDTO edificioDTO)
        {
            await _edificioService.Update(id, edificioDTO);
            return Ok(new { mensaje = "El edificio ha sido actualizado con éxito." });
        }

        // DELETE: api/Edificios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _edificioService.Delete(id);
            return Ok(new { mensaje = "El edificio ha sido eliminado con éxito." });
        }
    }
}
