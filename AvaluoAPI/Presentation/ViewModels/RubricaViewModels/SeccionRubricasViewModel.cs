using AvaluoAPI.Domain;

namespace AvaluoAPI.Presentation.ViewModels.RubricaViewModels
{
    public class SeccionRubricasViewModel
    {
        public string Seccion { get; set; } = null!;
        public string Asignatura { get; set; } = null!;
        public List<EstudianteDTO> Estudiantes { get; set; }
        public List<RubricaDashboardViewModel> Rubricas { get; set; }
    }
}
