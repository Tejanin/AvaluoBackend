using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Estado
    {
        public int Id { get; set; }
        public string IdTabla { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public virtual ICollection<Rubrica> Rubricas { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Asignatura> Asignaturas { get; set; }
        public virtual ICollection<Edificio> Edificios { get; set; }    
        public virtual ICollection<Inventario> Inventarios { get; set; }
        public virtual ICollection<Tarea> Tareas { get; set; }
        public virtual ICollection<ActionPlan> ActionPlans { get; set; }
        public virtual ICollection<Configuracion> Configuraciones { get;  set; }    
        public virtual ICollection<Competencia> Competencias { get; set; }
        public virtual ICollection<MapaCompetencias> MapaCompetencias { get; set; }
        public virtual ICollection<Aula> Aulas { get; set; }

    }
}
