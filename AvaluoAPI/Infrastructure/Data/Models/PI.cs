namespace Avaluo.Infrastructure.Data.Models
{
    public class PI
    {
        public virtual Competencia SO { get; set; } = null!;
        public string DescripcionES { get; set; } = null!;
        public string DescripcionEN { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int Id { get; set; }
        public int IdSO { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime? UltimaEdicion { get; set; }
        public virtual ICollection<Resumen> Resumenes { get; set; }
    }
}