using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Rubrica
    {
        public int Id { get; set; }
        public int IdSO { get; set; }
        public int IdProfesor { get; set; }
        public int IdAsignatura { get; set; }
        public int IdEstado { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public DateTime? UltimaEdicion { get; set; }
        public int CantEstudiantes { get; set; } = 0;
        public int Año { get; set; }
        public string Periodo { get; set; }
        public string Seccion { get; set; }
        public string Comentario { get; set; }
        public string Problematica { get; set; }
        public string Solucion { get; set; }
        public string Evidencia { get; set; } 
        public string EvaluacionesFormativas { get; set; }
        public string Estrategias { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Usuario Profesor { get; set; }
        public virtual Asignatura Asignatura { get; set; }
        public virtual Competencia SO { get; set; }
        public virtual ICollection<Resumen> Resumenes { get; set; }
        public virtual ICollection<Evidencia> Evidencias { get; set; }
        public virtual ICollection<CarreraRubrica> CarreraRubricas { get; set; }
        public virtual ICollection<ActionPlan> ActionPlans { get; set; }
        
    }
}
