using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Carrera
    {
        public int Id { get; set; }
        public int IdArea { get; set; }
        public int Año { get; set; }
        public string NombreCarrera { get; set; } = null!;
        public int? IdCoordinadorCarrera { get; set; }
        public string PEOs { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public int IdEstado { get; set; }
        public virtual Area Area { get; set; } = null!;
        public virtual Usuario? CoordinadorCarrera { get; set; } = null!;
        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<ProfesorCarrera> ProfesoresCarreras { get; set; }
        public virtual ICollection<Informe> Informes { get; set; }
        public virtual ICollection<CarreraRubrica> CarreraRubricas { get; set; }
        public virtual ICollection<AsignaturaCarrera> AsignaturaCarreras { get; set; }
    }
}
