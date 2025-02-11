using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.CompetenciaDTOs
{
    public class CompetenciaDTO
    {
        [Required(ErrorMessage = "El nombre de la competencia es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El acrónimo de la competencia es requerido.")]
        [StringLength(50, ErrorMessage = "El acrónimo no puede tener más de 50 caracteres.")]
        public string Acron { get; set; }

        [Required(ErrorMessage = "El título de la competencia es requerido.")]
        [StringLength(255, ErrorMessage = "El título no puede tener más de 255 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "La descripción en español es requerida.")]
        [StringLength(1000, ErrorMessage = "La descripción en español no puede tener más de 1000 caracteres.")]
        public string DescripcionES { get; set; }

        [Required(ErrorMessage = "La descripción en inglés es requerida.")]
        [StringLength(1000, ErrorMessage = "La descripción en inglés no puede tener más de 1000 caracteres.")]
        public string DescripcionEN { get; set; }

        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }

        [JsonIgnore]
        public DateTime UltimaEdicion { get; set; }

        [Required(ErrorMessage = "El estado de la competencia es requerido.")]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "El tipo de competencia es requerido.")]
        public int IdTipo { get; set; }
    }
}
