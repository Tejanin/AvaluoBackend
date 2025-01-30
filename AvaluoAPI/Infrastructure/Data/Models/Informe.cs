using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Informe
    {
        public int Id { get; set; }
        public string Ruta { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string Nombre { get; set; }
        public int IdTipo { get; set; }
        public int IdCarrera { get; set; }
        public DateTime Año { get; set; }
        public char Trimestre { get; set; }
        public string Periodo { get; set; }
        public virtual TipoInforme TipoInforme { get; set; }
        public virtual Carrera Carrera { get; set; }
    }
}
