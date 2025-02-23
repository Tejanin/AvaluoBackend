using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Presentation.DTOs.AsignaturaCarreraDTOs
{
    public class AsignaturaCarreraDTO
    {
        [Required(ErrorMessage = "El ID de la carrera es obligatorio.")]
        public int IdCarrera { get; set; }

        [Required(ErrorMessage = "El ID de la asignatura es obligatorio.")]
        public int IdAsignatura { get; set; }
    }
}
