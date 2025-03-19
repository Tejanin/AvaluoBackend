namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class ModifyUsuarioDTO
    {

        public string? Username { get; set; } 
  
        public string? Email { get; set; } 
  
        public string? Nombre { get; set; } 
   
        public string? Apellido { get; set; } 

        public int? Estado { get; private set; } 
        public int? Area { get; set; }
        public int? Rol { get; set; }
    }
}
