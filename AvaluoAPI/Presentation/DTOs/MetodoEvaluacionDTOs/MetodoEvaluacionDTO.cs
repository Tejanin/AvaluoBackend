using Avaluo.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs
{
    public class MetodoEvaluacionDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "La descripción del metodo de evaluacion es requerida")]
        [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
    }
}
