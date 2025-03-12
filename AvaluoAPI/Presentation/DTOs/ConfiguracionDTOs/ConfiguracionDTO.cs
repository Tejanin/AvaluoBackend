using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.ConfiguracionDTOs
{
    public class ConfiguracionDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "La descripcion de la configuracion es requerida.")]
        [StringLength(255, ErrorMessage = "La descripcion no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La fecha de inicio es requerida.")]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "La fecha de cierre es requerida.")]
        public DateTime FechaCierre { get; set; }
        [Required(ErrorMessage = "El estado es requerido.")]
        public int IdEstado { get; set; }
    }
}
