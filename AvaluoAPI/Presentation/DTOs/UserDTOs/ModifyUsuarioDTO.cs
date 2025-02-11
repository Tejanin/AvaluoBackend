namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class ModifyUsuarioDTO
    {

        public string? Username { get; set; } 
  
        public string? Email { get; set; } 
  
        public string? Nombre { get; set; } 
   
        public string? Apellido { get; set; } 

        public int? IdEstado { get; private set; } 
        public int? IdArea { get; set; }
        public int? IdRol { get; set; }
    }
}
