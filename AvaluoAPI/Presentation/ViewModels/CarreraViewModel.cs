using Avaluo.Infrastructure.Data.Models;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class CarreraViewModel
    {
        public int Id { get; set; }
        public int Año { get; set; }
        public string NombreCarrera { get; set; }
        public string PEOs { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? UltimaEdicion { get; set; }
        public virtual AreaViewModel Area { get; set; }
        public virtual UsuarioViewModel CoordinadorCarrera { get; set; }
        public virtual EstadoViewModel Estado { get; set; }
    }
}
