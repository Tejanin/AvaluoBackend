using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Inventario
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        [Column("Id_Estado")]
        public int IdEstado { get; set; }
        public virtual ICollection<ObjetoAula> ObjetoPorAula { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
