using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.AsignaturaDTOs
{
    public class AsignaturaModifyDTO
    {
        [Required(ErrorMessage = "El número de créditos es requerido.")]
        public int Creditos { get; set; }

        [Required(ErrorMessage = "El código de la asignatura es requerido.")]
        [StringLength(50, ErrorMessage = "El código no puede tener más de 50 caracteres.")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre de la asignatura es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; }

        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }

        [JsonIgnore]
        public DateTime UltimaEdicion { get; set; }

        [Required(ErrorMessage = "El estado de la asignatura es requerido.")]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "El área de la asignatura es requerida.")]
        public int IdArea { get; set; }
    }
}
