namespace AvaluoAPI.Presentation.ViewModels
{
    public class CompetenciaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaEdicion { get; set; }
        public string TipoCompetencia { get; set; } 
        public string Estado { get; set; } 
    }
}
