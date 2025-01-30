using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class SOEvaluacion
    {
        public int IdMetodoEvaluacion { get; set; }
        public int IdSO { get; set; }
        public virtual MetodoEvaluacion MetodoEvaluacion { get; set; }
        public virtual Competencia SO { get; set; }
    }
}
