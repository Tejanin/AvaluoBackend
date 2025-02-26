namespace AvaluoAPI.Domain
{
    public class ProfesorCarrerasDTO
    {
        public int IdProfesor { get; set; }
        public int? IdSO { get; set; }
        public List<int> CarrerasIds { get; set; } = new List<int>();
    }
}
