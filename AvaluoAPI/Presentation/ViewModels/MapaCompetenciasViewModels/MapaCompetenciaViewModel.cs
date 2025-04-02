namespace AvaluoAPI.Presentation.ViewModels.MapaCompetenciasViewModels
{
    public class MapaCompetenciaViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string ProgramaAsignatura { get; set; }
        public string Syllabus { get; set; }
        public EstadoViewModel Estado { get; set; }
        public List<MapaCompetenciaResumenViewModel> Competencias { get; set; }
    }

    public class MapaCompetenciaResumenViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Acron { get; set; }
        public EstadoViewModel Estado { get; set; }
    }
}
