namespace AvaluoAPI.Infrastructure.Integrations.Moodle.Models
{
    public class EvidenciasMoodleModel
    {
        public string Id { get; set; } = null!;
        public string Nombre { get; set; } = null!; 
        public List<EstudianteMoodleModel> Estudiantes { get; set; } = null!;
    }
}
