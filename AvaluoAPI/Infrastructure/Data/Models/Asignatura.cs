using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Asignatura
    {
        public int Id { get; set; }
        public int Creditos { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public int IdEstado { get; set; }
        public string ProgramaAsignatura { get; set; }
        public string Syllabus { get; set; }
        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<AsignaturaCarrera> AsignaturaCarreras { get; set; }
        public virtual ICollection<MapaCompetencias> MapaCompetencias { get; set; }
        public virtual ICollection<Desempeno> Desempenos { get; set; }
        public virtual ICollection<Rubrica> Rubricas { get; set; }
    }
}
