using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class UsuarioDTO
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = null!;
        public string Salt { get; private set; } = DateTime.Now.ToString();
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = null!;
 
        public int IdEstado { get; private set; } = 5;
        public int? IdArea { get; set; }
        public int? IdRol { get; set; }
    }
}
