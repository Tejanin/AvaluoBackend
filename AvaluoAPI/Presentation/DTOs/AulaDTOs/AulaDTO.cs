using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.AulaDTOs
{
    public class AulaDTO
    {
        private int Id { get; set; }

        [Required(ErrorMessage = "La descripcion del aula es requerida.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }
        [JsonIgnore]
        public DateTime UltimaEdicion { get; set; }
        [Required(ErrorMessage = "El edificio del aula es requerida.")]
        public int IdEdificio { get; set; }
        [Required(ErrorMessage = "El estado del aula es requerido.")]
        public int IdEstado { get; set; }
    }
}
