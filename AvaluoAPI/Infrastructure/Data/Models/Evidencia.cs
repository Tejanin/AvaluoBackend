using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Evidencia
    {
        public int Id { get; set; }
        public int IdRubrica { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public virtual Rubrica Rubrica { get; set; }
    }
}
