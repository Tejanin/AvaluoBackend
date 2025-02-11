namespace AvaluoAPI.Presentation.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }  
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Area { get; set; } = null!;
        public string Rol { get; set; } = "No asignado";
        public string? SO { get; set; }
        public string? CV { get; set; }
        public string? Foto { get; set; }
        public List<ContactoViewModel>? Contactos { get; set; }



    }
}
