namespace AvaluoAPI.Presentation.ViewModels
{
    public class CompetenciaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Acron { get; set; }
        public string Titulo { get; set; }
        public string DescripcionES { get; set; }
        public string DescripcionEN { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaEdicion { get; set; }
        public TipoCompetenciaViewModel TipoCompetencia { get; set; }
        public EstadoViewModel Estado { get; set; }
    }
}