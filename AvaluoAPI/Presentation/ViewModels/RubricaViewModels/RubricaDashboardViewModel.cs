using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.ViewModels.RubricaViewModels
{
    public class RubricaDashboardViewModel
    {
        public int Id { get; set; }
        public SOwithPIsViewModel SO { get; set; }
        [JsonIgnore]
        public string Seccion { get; set; }
        [JsonIgnore]
        public int IdAsignatura { get; set; }
        [JsonIgnore]
        public string AsignaturaCodigo { get; set; }  // Código de la asignatura
        [JsonIgnore]
        public string AsignaturaNombre { get; set; }  // Nombre de la asignatura
        public string Estado { get; set; }
        public string Comentario { get; set; }
        public string Problematica { get; set; }
        public string Solucion { get; set; }
        public string Evidencia { get; set; }
        public string EvaluacionesFormativas { get; set; }
        public string Estrategias { get; set; }
        public List<ResumenViewModel> Resumenes { get; set; }
    }
}
