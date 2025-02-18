using AvaluoAPI.Domain.Services.RubricasService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvaluoAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RubricaController : ControllerBase
    {
        private readonly IRubricaService _rubricaService;
        public RubricaController(IRubricaService rubricaService) 
        {
            _rubricaService = rubricaService;   
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
           var rubricas = await _rubricaService.InsertRubricas();
            return Ok(new { message = "Operación exitosa", data = rubricas });
        }
    }
}
