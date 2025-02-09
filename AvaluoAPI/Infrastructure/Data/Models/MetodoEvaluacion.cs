

namespace Avaluo.Infrastructure.Data.Models
{
    public class MetodoEvaluacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } =null!;
        public DateTime? UltimaEdicion { get; set; } 
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public virtual ICollection<SOEvaluacion> SOEvaluaciones { get; set; }
    }


    
}
