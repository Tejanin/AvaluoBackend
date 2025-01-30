using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class ObjetoAula
    {
        public int IdObjeto { get; set; }
        public int IdAula { get; set; }
        public int Cantidad { get; set; } 
        public virtual Aula Aula { get; set; }
        public virtual Inventario Inventario { get; set; }
    }
}
