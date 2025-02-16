namespace AvaluoAPI.Infrastructure.Integrations.INTEC.Models
{
    public class PersonModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cargo { get; set; }

    }

    public class  EstudianteModel : PersonModel
    {
        
    }

    public class ProfesorModel : PersonModel
    {
        public List<SeccionModel>? Secciones { get; set; }

    }


}
