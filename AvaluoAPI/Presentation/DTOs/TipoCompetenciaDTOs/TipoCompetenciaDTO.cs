using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs
{
    public class TipoCompetenciaDTO
    {
        // public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del tipo de competencia es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; }
        // public DateTime FechaCreacion { get; set; }
        // public DateTime UltimaEdicion { get; set; }
    }
}
