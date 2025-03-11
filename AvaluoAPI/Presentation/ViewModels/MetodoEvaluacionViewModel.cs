namespace AvaluoAPI.Presentation.ViewModels
{
    public class MetodoEvaluacionViewModel
    {
        public int Id { get; set; }
        public string DescripcionES { get; set; } = null!;
        public string DescripcionEN { get; set; } = null!;
        public DateTime? UltimaEdicion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
