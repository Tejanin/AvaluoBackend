using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class AsignaturaCarrera
    {
        public int IdAsignatura { get; set; }
        public int IdCarrera { get; set; }
        public virtual Asignatura Asignatura { get; set; }
        public virtual Carrera Carrera { get; set; }

        
    }
}
