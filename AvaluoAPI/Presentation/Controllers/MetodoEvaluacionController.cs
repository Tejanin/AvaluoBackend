using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Domain.Services.MetodoEvaluacionService;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodoEvaluacionController : ControllerBase
    {
        private readonly IMetodoEvaluacionService _metodoEvaluacionService;

        public MetodoEvaluacionController(IMetodoEvaluacionService metodoEvaluacionService)
        {
            _metodoEvaluacionService = metodoEvaluacionService;
        }

        // GET: api/MetodoEvaluacion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetodoEvaluacionViewModel>>> Get()
        {
            var metodos = await _metodoEvaluacionService.GetAll();
            return Ok(new { mensaje = "Metodos de evaluacion obtenidos exitosamente.", data = metodos });
        }

        // GET: api/MetodoEvaluacion/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MetodoEvaluacionViewModel>> Get(int id)
        {
            var tipo = await _metodoEvaluacionService.GetById(id);
            return Ok(new { mensaje = "Tipo de informe encontrado.", data = tipo });
        }

        // POST: api/MetodoEvaluacion
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MetodoEvaluacionDTO metodoEvaluacionDTO)
        {
            await _metodoEvaluacionService.Register(metodoEvaluacionDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Metodo de evaluacion creado exitosamente." });
        }

        // PUT: api/MetodoEvaluacion/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] MetodoEvaluacionDTO metodoEvaluacionDTO)
        {
            await _metodoEvaluacionService.Update(id, metodoEvaluacionDTO);
            return Ok(new { mensaje = "El metodo de evaluacion ha sido actualizado con éxito." });
        }

        // DELETE: api/MetodoEvaluacion/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _metodoEvaluacionService.Delete(id);
            return Ok(new { mensaje = "El metodo de evaluacion ha sido eliminado con éxito." });
        }
    }
}
