namespace AvaluoAPI.Presentation.DTOs.UserDTOs
{
    public class TipoInformeDTO
    {
        // Comenté el ID porque la base de datos lo autogenera, así que no necesito enviarlo desde el API
        // public int Id { get; set; } 
        public string Descripcion { get; set; }
    }
}
