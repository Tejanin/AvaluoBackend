namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class UsuarioDTO
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; private set; } = DateTime.Now.ToString();
        
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
 
        public int IdEstado { get; private set; } = 5;
        public int? IdArea { get; set; }
        public int? IdRol { get; set; }
    }
}
