using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class TipoInformeDTO
    {
        // Comenté el ID porque la base de datos lo autogenera, así que no necesito enviarlo desde el API
        // public int Id { get; set; } 
        [Required(ErrorMessage = "La descripción del tipo de informe es requerida")]
        [StringLength(255, ErrorMessage = "La descripción no puede tener más de 255 caracteres.")]
        public string Descripcion { get; set; }
    }
}
