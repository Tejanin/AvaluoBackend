namespace AvaluoAPI.Domain
{
    public class EstudianteDTO
    {
        public string Matricula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        
        public int Calificacion { get; set; } = 1;

    }
}
