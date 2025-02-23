namespace AvaluoAPI.Presentation.ViewModels
{
    public class RubricaViewModel
    {
        public int Id { get; set; }
        public string Comentario { get; set; }
        public string Problematica { get; set; }
        public string Solucion { get; set; }
        public string Evidencia { get; set; }
        public string EvaluacionesFormativas { get; set; }
        public string Estrategias { get; set; }
        public List<ResumenViewModel> Resumenes { get; set; }

    }
}
