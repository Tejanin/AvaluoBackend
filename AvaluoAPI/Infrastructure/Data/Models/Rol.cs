using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public bool EsProfesor { get; set; } = false;
        public bool EsSupervisor { get; set; } = false;
        public bool EsCoordinadorArea { get; set; } = false;
        public bool EsCoordinadorCarrera { get; set; } = false;
        public bool EsAdmin { get; set; } = false;
        public bool EsAux { get; set; } = false;
        public bool VerInformes { get; set; } = false;
        public bool VerListaDeRubricas { get; set; } = false;
        public bool ConfigurarFechas { get; set; } = false;
        public bool VerManejoCurriculum { get; set; } = false;

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
