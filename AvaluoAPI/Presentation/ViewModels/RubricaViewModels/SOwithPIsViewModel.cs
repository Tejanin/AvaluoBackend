namespace AvaluoAPI.Presentation.ViewModels.RubricaViewModels
{
    public class SOwithPIsViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Acron { get; set; }
        public string DescripcionES { get; set; }
        public List<PIViewModel> PIs { get; set; }
    }
}
