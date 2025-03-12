namespace AvaluoAPI.Presentation.ViewModels.CofiguracionViewModels
{
    public class ConfiguracionViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaCierre { get; set; }
        public string Estado { get; set; }
    }
}
