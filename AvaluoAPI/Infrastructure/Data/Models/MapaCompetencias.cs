using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class MapaCompetencias
    {
        public int IdAsignatura { get; set; }
        public int IdCompetencia { get; set; }
        public int IdEstado { get; set; }   
        public virtual Estado Estado { get; set; }
        public virtual Asignatura Asignatura { get; set; }
        public virtual Competencia Competencia { get; set; }
    }
}
