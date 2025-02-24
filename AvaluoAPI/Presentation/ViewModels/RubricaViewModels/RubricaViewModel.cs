namespace AvaluoAPI.Presentation.ViewModels.RubricaViewModels
{

    public class RubricaViewModel
    {
        public int Id { get; set; }
        public string Profesor { get; set; }  // Nuevo campo
        public CarreraRubricaViewModel Carrera { get; set; }
        public SORubricaViewModel SO { get; set; }
        public AsignaturaRubricaViewModel Asignatura { get; set; }
        public EstadoViewModel Estado { get; set; }
        public string Comentario { get; set; }
        public string Problematica { get; set; }
        public string Solucion { get; set; }
        public string Evidencia { get; set; }
        public string EvaluacionesFormativas { get; set; }
        public string Estrategias { get; set; }
        public List<ResumenViewModel> Resumenes { get; set; }
    }


}
