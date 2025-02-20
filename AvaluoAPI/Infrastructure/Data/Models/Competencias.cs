using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Competencia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Acron { get; set; } = null!; 
        public string Titulo { get; set; } = null!; 
        public string DescripcionES { get; set; } = null!;
        public string DescripcionEN { get; set; } = null!; 
        public int IdTipo { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public int IdEstado { get; set; }

        // Relaciones con otras entidades
        public virtual TipoCompetencia TipoCompetencia { get; set; }
        public virtual Estado Estado { get; set; }
        public virtual ICollection<MapaCompetencias> MapaCompetencias { get; set; }
        public virtual ICollection<Rubrica> Rubricas { get; set; }
        public virtual ICollection<SOEvaluacion> SOEvaluaciones { get; set; }
        public virtual ICollection<Desempeno> Desempenos { get; set; }
        public virtual ICollection<PI> PIs { get; set; }
        public virtual ICollection<Usuario> Profesores { get; set; }
    }
}
