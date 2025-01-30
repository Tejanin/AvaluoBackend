using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public int IdEstado { get; set; }
        public virtual ICollection<ObjetoAula> ObjetoPorAula { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
