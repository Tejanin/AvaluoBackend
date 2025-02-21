using Avaluo.Infrastructure.Data.Models;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class AulaViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaEdicion { get; set; }
        public EdificioViewModel Edificio { get; set; }
        public EstadoViewModel Estado { get; set; }
    }
}
