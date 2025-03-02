namespace AvaluoAPI.Presentation.ViewModels
{
    public class AsignaturaConCompetenciasViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public EstadoViewModel Estado { get; set; } 
        public List<CompetenciaResumenViewModel> Competencias { get; set; }
    }

    public class CompetenciaResumenViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Acron { get; set; }
        public EstadoViewModel Estado { get; set; }
    }
}
