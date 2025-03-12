namespace AvaluoAPI.Presentation.ViewModels
{
    public class InformeDesempeñoViewModel
    {
        public int IdAsignatura { get; set; }
        public string CodigoAsignatura { get; set; } = string.Empty;
        public string NombreAsignatura { get; set; } = string.Empty;
        public int Año { get; set; }
        public string Trimestre { get; set; } = string.Empty;
        public int TotalEstudiantes { get; set; }

        public List<StudentOutcomeViewModel> StudentOutcomes { get; set; } = new List<StudentOutcomeViewModel>();
    }

    public class StudentOutcomeViewModel
    {
        public int IdSO { get; set; }
        public string NombreSO { get; set; } = string.Empty;
        public string DescripcionSO { get; set; } = string.Empty;

        public List<PerformanceIndicatorViewModel> PerformanceIndicators { get; set; } = new List<PerformanceIndicatorViewModel>();
    }

    public class PerformanceIndicatorViewModel
    {
        public int IdPI { get; set; }
        public string DescripcionPI { get; set; } = string.Empty;

        public int CantExperto { get; set; }
        public int CantSatisfactorio { get; set; }
        public int CantPrincipiante { get; set; }
        public int CantDesarrollo { get; set; }

        public int TotalEstudiantes => CantExperto + CantSatisfactorio + CantPrincipiante + CantDesarrollo;

        public decimal PorcentajeSatisfactorio { get; set; }
        public bool EsSatisfactorio { get; set; }
    }
}
