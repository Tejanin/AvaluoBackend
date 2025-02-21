namespace AvaluoAPI.Presentation.ViewModels
{
    public class AsignaturaViewModel
    {
        public int Id { get; set; }
        public int Creditos { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimaEdicion { get; set; }
        public string ProgramaAsignatura { get; set; }
        public string Syllabus { get; set; }
        public EstadoViewModel Estado { get; set; } = new EstadoViewModel();
        public AreaViewModel Area { get; set; } = new AreaViewModel();
    }

}
