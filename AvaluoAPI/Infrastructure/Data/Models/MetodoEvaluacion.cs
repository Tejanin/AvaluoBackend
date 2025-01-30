using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class MetodoEvaluacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime UltimaEdicion { get; set; } = DateTime.Now;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public virtual ICollection<SOEvaluacion> SOEvaluaciones { get; set; }
    }


    
}
