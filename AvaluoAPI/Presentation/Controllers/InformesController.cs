using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformesController : ControllerBase
    {
        public InformesController()
        {
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInforme(int id)
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInformes(int? tipo, int? año, int? trimestre, int? periodo, int? recordsPerPage )
        {
            return Ok();
        }


    }
}
