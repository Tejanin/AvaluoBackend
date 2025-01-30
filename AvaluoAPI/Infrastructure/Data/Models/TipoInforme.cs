using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class TipoInforme
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public virtual ICollection<Informe>? Informes { get; set; }
    }
}
