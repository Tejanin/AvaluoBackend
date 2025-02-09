using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.CompetenciaDTOs
{
    public class CompetenciaDTO
    {
        // [JsonIgnore]
        // public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la competencia es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción de la competencia es requerida.")]
        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo de competencia es requerido.")]
        public int IdTipo { get; set; } 

        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }

        [JsonIgnore]
        public DateTime UltimaEdicion { get; set; }

        [Required(ErrorMessage = "El estado de la competencia es requerido.")]
        public int IdEstado { get; set; }
    }
}
