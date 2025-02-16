namespace AvaluoAPI.Infrastructure.Integrations.Moodle.Models
{
    public class ListaDeEvidenciasMoodleModel
    {
        public string CodigoAsignatura { get; set; } = null!;
        public string Seccion { get; set; } = null!;
        public List<EvidenciasMoodleModel> NombreDeEvidencias { get; set; } = null!;
    }
}
