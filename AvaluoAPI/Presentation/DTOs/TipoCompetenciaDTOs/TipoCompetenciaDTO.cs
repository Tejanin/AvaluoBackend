using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs
{
    public class TipoCompetenciaDTO
    {
        [JsonIgnore]
        public int Id { get; set; } 

        [Required(ErrorMessage = "El nombre del tipo de competencia es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; }

        [JsonIgnore]
        public DateTime FechaCreacion { get; set; } 

        [JsonIgnore]
        public DateTime UltimaEdicion { get; set; } 
    }
}
