using Avaluo.Infrastructure.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.EdificioDTOs
{
    public class EdificioDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "El Area es requerido.")]
        public int IdArea { get; set; }
        [Required(ErrorMessage = "El Estado es requerido.")]
        public int IdEstado { get; set; }
        [Required(ErrorMessage = "El nombre del edificio es requerido.")]
        [StringLength(255, ErrorMessage = "El nombre no puede tener más de 255 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El acronimo es requerido.")]
        public string Acron { get; set; }
        [Required(ErrorMessage = "La ubicacion es requerido.")]
        public string Ubicacion { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [JsonIgnore]
        public DateTime UltimaEdicion { get; set; }
    }
}
