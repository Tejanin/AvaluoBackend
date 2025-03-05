using AvaluoAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AvaluoAPI.Presentation.DTOs.RubricaDTOs
{
    public class CompleteRubricaDTO
    {

        public int Id { get; set; } 
        public string Comentario { get; set; }
        public string Problematica { get; set; }
        public int MetodoEvaluacion { get; set; }
        public string Solucion { get; set; }
        public string Evidencia { get; set; }
        public string EvaluacionesFormativas { get; set; }
        public string Estrategias { get; set; }
        public List<ResumenDTO> Resumenes { get; set; }
    }

    public class CompleteRubricaFormDTO
    {
        [FromForm]
        public string JsonData { get; set; }

        [FromForm]
        public List<IFormFile>? Evidencias { get; set; }

        public CompleteRubricaDTO GetRubricaDTO()
        {
            return JsonSerializer.Deserialize<CompleteRubricaDTO>(JsonData)!;
        }
    }
}
