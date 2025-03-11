using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.ContactoDTOs
{
    public class ContactoDTO
    {
        [Required(ErrorMessage = "El número de contacto es requerido.")]

        [StringLength(20, ErrorMessage = "El número de contacto no puede tener más de 20 caracteres.")]
        public string NumeroContacto { get; set; } = null!;

        public int IdUsuario { get; set; }
    }
}
