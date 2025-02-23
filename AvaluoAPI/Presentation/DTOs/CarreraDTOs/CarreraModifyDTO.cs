using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.CarreraDTOs
{
    public class CarreraModifyDTO
    {
        [Required(ErrorMessage = "El año de la carrera es requerido.")]
        public int Año { get; set; }

        [Required(ErrorMessage = "El nombre de la carrera es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string NombreCarrera { get; set; }

        [Required(ErrorMessage = "La descripción de PEOs es requerida.")]
        public string PEOs { get; set; }

        [Required(ErrorMessage = "El ID del estado es requerido.")]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "El ID del área es requerido.")]
        public int IdArea { get; set; }

        [Required(ErrorMessage = "El ID del coordinador de la carrera es requerido.")]
        public int IdCoordinadorCarrera { get; set; }

        [JsonIgnore]
        public DateTime? UltimaEdicion { get; set; }
    }
}
