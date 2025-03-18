using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contraseña { get; set; } = null!;
    }
}
