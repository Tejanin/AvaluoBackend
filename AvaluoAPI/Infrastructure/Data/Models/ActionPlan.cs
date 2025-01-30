using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class ActionPlan
    {
        public int Id { get; set; }
        public int IdDesempeno { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public int IdRubrica { get; set; }   
        public string Descripcion { get; set; } = null!;
        public int IdEstado { get; set; } = 1;
        public virtual Desempeno Desempeno { get; set; }
        public virtual ICollection<Tarea> Tareas { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Rubrica Rubrica { get; set; } = null!;
    }
}
