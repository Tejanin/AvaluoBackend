using AvaluoAPI.Domain.Services.PIService;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PIController : ControllerBase
    {
        private readonly IPiService _piService;
        public PIController(IPiService piService)
        {
            _piService = piService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PIViewModel>> Get(int id)
        {
            return Ok(await _piService.GetPI(id));    
        }

        [HttpGet("SO/{so}")]
        public async Task<ActionResult<IEnumerable<PIViewModel>>> GetPIsBySO(int so)
        {
            return Ok(await _piService.GetPIsBySO(so));
        }
    }
}
