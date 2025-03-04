using AvaluoAPI.Domain;

namespace AvaluoAPI.Presentation.DTOs.RubricaDTOs
{
    public class ResumenDTO
    {
        public int IdPI { get; set; }

        public List<EstudianteDTO> Estudiantes { get; set; }
    }
}
