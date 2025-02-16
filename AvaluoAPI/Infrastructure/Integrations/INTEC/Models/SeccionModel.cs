namespace AvaluoAPI.Infrastructure.Integrations.INTEC.Models
{
    public class SeccionModel
    {
        public string Numero { get; set; }
        
        public string Asignatura { get; set; }
        public List<PersonModel> Estudiantes { get; set; }
    }
}
