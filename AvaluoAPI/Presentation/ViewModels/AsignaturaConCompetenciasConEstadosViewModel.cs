namespace AvaluoAPI.Presentation.ViewModels
{
    public class AsignaturaConCompetenciasConEstadosViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public EstadoViewModel Estado { get; set; } 
        public List<CompetenciaResumenConEstadosViewModel> Competencias { get; set; }
    }

    public class CompetenciaResumenConEstadosViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Acron { get; set; }
        public EstadoViewModel Estado { get; set; }
    }
}
