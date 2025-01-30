using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avaluo.Infrastructure.Data.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string HashedPassword { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? UltimaEdicion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Foto { get; set; }
        public string? CV { get; set; }
        public int? IdSO { get; set; }
        public int IdEstado { get; set; }
        public int? IdArea { get; set; }
        public int? IdRol { get; set; }
        public virtual Area Area { get; set; } = null!;
        public virtual Rol Rol { get; set; } = null!;
        public virtual Competencia? SO { get; set; }
        public virtual Estado Estado { get; set; } = null!;
        public virtual ICollection<Contacto> Contactos { get; set; } = null!;
        public virtual ICollection<HistorialIncumplimiento>? HistorialIncumplimientos { get; set; } 
        public virtual ICollection<Rubrica>? Rubricas { get; set; }
        public virtual ICollection<Tarea>? Tareas { get; set; }
        public virtual ICollection<ProfesorCarrera>? ProfesoresCarreras { get; set; }
    }
}
