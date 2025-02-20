using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Edificio
    {
        public int Id { get; set; }
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public string Acron { get; set; }
        public int IdEstado { get; set; }
        public string Ubicacion { get; set; }
        public virtual Area Area { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<Aula> Aulas { get; set; }
    }
}
