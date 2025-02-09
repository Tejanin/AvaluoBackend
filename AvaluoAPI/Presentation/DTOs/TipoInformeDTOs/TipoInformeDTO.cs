using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class TipoInformeDTO
    {
        [JsonIgnore]
        public int Id { get; set; } 

        [Required(ErrorMessage = "La descripción del tipo de informe es requerida")]
        [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
    }
}
