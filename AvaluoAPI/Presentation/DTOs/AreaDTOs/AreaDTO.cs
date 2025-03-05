using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.AreaDTOs
{
    public class AreaDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int? IdCoordinador { get; set; }
        [Required(ErrorMessage = "La descripcion del area es requerido.")]
        [StringLength(255, ErrorMessage = "La descripcion no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
    }
}
