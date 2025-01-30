namespace Avaluo.Infrastructure.Data.Models
{
    public class Resumen
    {
        public int CantDesarrollo { get; set; } = 0;
        public int CantExperto { get; set; } = 0;
        public int CantPrincipiante { get; set; } = 0;
        public int CantSatisfactorio { get; set; } = 0;
        public int IdPI { get; set; }
        public int IdRubrica { get; set; }
        public virtual PI PI { get; set; }
        public virtual Rubrica Rubrica { get; set; }
    }
}