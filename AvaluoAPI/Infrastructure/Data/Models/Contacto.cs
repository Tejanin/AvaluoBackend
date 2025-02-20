using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Contacto
    {
        public int Id { get; set; }
        public string NumeroContacto { get; set; } = null!;
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
