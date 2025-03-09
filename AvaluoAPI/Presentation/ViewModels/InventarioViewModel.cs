namespace AvaluoAPI.Presentation.ViewModels
{
    public class InventarioViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimaEdicion { get; set; }
        public int IdEstado { get; set; }
        public int CantidadTotal { get; set; }
    }
}
