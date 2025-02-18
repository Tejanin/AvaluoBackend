namespace AvaluoAPI.Presentation.DTOs.RubricaDTOs
{
    public class CreateRubricaDTO
    {

        public int IdSO { get; set; } //
        public int IdProfesor { get; set; } //
        public int IdAsignatura { get; set; } // Se determina por el servicio
        public int IdEstado { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public DateTime? UltimaEdicion { get; set; }
        public int CantEstudiantes { get; set; } = 0;
        public int Año { get; set; } // se tiene que determinar por el año universitario
        public string Periodo { get; set; } // se tiene que determinar por el trimestre universitario
        public string Seccion { get; set; } // se determina por el servicio
        public string Comentario { get; set; }
        public string Problematica { get; set; }
        public string Solucion { get; set; }
        public string EvaluacionesFormativas { get; set; }
        public string Estrategias { get; set; }
    }
}
