using AvaluoAPI.Domain.Services.HistorialIncumplimientoService;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialIncumplimientoController : ControllerBase
    {
        private readonly IHistorialIncumplimientoService _historialIncumplimientoService;
        public HistorialIncumplimientoController(IHistorialIncumplimientoService historialIncumplimientoService)
        {
            _historialIncumplimientoService = historialIncumplimientoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] int? idUsuario,
        [FromQuery] string? descripcion,
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        [FromQuery] int? page,
        [FromQuery] int? recordsPerPage)
        {
            var result = await _historialIncumplimientoService.GetAll(idUsuario, descripcion, desde, hasta, page, recordsPerPage);
            return Ok(new { mensaje = "Historial obtenido exitosamente.", data = result });
        }

    }
}
