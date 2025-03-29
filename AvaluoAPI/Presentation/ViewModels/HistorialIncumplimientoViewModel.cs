using Avaluo.Infrastructure.Data.Models;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class HistorialIncumplimientoViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Usuario { get; set; }
       // public int IdUsuario { get; set; }
       // public virtual Usuario Usuario { get; set; }
    }
}
