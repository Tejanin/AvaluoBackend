using Avaluo.Infrastructure.Data.Models;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class AreaViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Usuario Coordinador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime UltimaEdicion { get; set; }
    }
}
