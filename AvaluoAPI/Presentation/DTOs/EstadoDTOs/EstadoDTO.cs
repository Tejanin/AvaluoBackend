using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.EstadoDTOs
{
    public class EstadoDTO
    {
        [Required(ErrorMessage = "La tabla es requerida.")]
        [StringLength(255, ErrorMessage = "El nombre de la tabla no puede tener más de 255 caracteres.")]
        public string IdTabla { get; set; }
        [Required(ErrorMessage = "La descripcion es requerido.")]
        [StringLength(255, ErrorMessage = "La descripcion no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
    }
}
