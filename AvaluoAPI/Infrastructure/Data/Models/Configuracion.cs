using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Configuracion
    {
        public int Id { get; set; }
        public string Año { get; set; }
        public string Trimestre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }
        public int IdEstado { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
