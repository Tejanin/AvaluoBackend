using AvaluoAPI.Domain.Services.RubricasService;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
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

        [HttpPost]
        public async Task<IActionResult> CompleteRubricas(CompleteRubricaFormDTO rubricaDTO)
        {
            await _rubricaService.CompleteRubricas(rubricaDTO.GetRubricaDTO(),rubricaDTO.Evidencias);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditRubricas(CompleteRubricaFormDTO rubricaDTO)
        {
            await _rubricaService.EditRubricas(rubricaDTO.GetRubricaDTO(), rubricaDTO.Evidencias);
            return Ok();
        }
    }
}
