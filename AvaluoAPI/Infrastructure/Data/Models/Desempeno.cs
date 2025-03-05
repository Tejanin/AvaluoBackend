using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Desempeno
    {
        public int Id { get; set; }
        public int IdSO { get; set; }
        public int IdPI { get; set; }
        public int IdAsignatura { get; set; }
        public bool Satisfactorio { get; set; } = false;
        public char Trimestre { get; set; }
        public int Año { get; set; }
        public decimal Porcentaje { get; set; }
        public virtual Asignatura Asignatura { get; set; }
        public virtual Competencia SO { get; set; }
        public virtual PI PI { get; set; }
        public virtual ICollection<ActionPlan> ActionPlan { get; set; }
    }
}
