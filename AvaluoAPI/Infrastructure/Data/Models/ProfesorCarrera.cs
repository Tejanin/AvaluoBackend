using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class ProfesorCarrera
    {
        public int IdProfesor { get; set; }
        public int IdCarrera { get; set; }
        public virtual Carrera Carrera { get; set; }
        public virtual Usuario Profesor { get; set; }
    }
}
