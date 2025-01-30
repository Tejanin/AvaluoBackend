using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        public int IdActionPlan { get; set; }
        public int IdAuxiliar { get; set; }
        public int IdEstadoTarea { get; set; } = 1;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public string Descripcion { get; set; }
        public virtual ActionPlan ActionPlan { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual Usuario Auxiliar { get; set; }
    }
}
