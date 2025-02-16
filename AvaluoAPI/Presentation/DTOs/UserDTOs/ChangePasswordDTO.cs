using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class ChangePasswordDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string NewPassword { get; set; } = null!;
    }
}
