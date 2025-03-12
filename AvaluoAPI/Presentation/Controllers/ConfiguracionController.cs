using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Domain.Services.ConfiguracionService;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.DTOs.ConfiguracionDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.CofiguracionViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracionController : ControllerBase
    {
        private readonly IConfiguracionService _configuracionService;

        public ConfiguracionController(IConfiguracionService configuracionService)
        {
            _configuracionService = configuracionService;
        }


        // GET: api/Configuracion
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ConfiguracionViewModel>>> Get(
            [FromQuery] string? descripcion,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaCierre,
            [FromQuery] int? idEstado,
            [FromQuery] int? page,
            [FromQuery] int? recordsPerPage)
        {
            var configuracion = await _configuracionService.GetAll(descripcion, fechaInicio, fechaCierre, idEstado, page, recordsPerPage);
            return Ok(new { mensaje = "Configuraciones obtenidas exitosamente.", data = configuracion });
        }


        // GET: api/Configuracion/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ConfiguracionViewModel>> Get(int id)
        {
            var configuracion = await _configuracionService.GetById(id);
            return Ok(new { mensaje = "Configuracion encontrada.", data = configuracion });
        }

        // POST: api/Configuracion
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ConfiguracionDTO configuracionDTO)
        {
            await _configuracionService.Register(configuracionDTO);
            return CreatedAtAction(nameof(Get), new { mensaje = "Configuracion creada exitosamente." });
        }

        // PUT: api/Configuracion/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ConfiguracionDTO configuracionDTO)
        {
            await _configuracionService.Update(id, configuracionDTO);
            return Ok(new { mensaje = "La configuracion ha sido actualizada con éxito." });
        }

        // GET: api/Configuracion/{id}
        [HttpGet("Fechas")]
        public async Task<ActionResult<FechaConfiguracionViewModel>> Get()
        {
            var configuracion = await _configuracionService.GetFechas();
            return Ok(new { mensaje = "Fechas encontradas.", data = configuracion });
        }
    }
}
