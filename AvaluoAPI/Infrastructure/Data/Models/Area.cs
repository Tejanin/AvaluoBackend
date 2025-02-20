using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Area
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int? IdCoordinador { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public virtual Usuario Coordinador { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Carrera> Carreras { get; set; }
        public virtual ICollection<Edificio> Edificios { get; set; }
        public virtual ICollection<Asignatura> Asignaturas { get; set; }
    }
}
