namespace AvaluoAPI.Presentation.ViewModels.DashboardsViewModels
{
    using System.Collections.Generic;

    namespace AvaluoAPI.Presentation.ViewModels
    {
        // ViewModel para toda la estructura del reporte
        public class ReporteSOViewModel
        {
            public string Carrera { get; set; }
            public List<SOResumenViewModel> Resumen { get; set; } = new List<SOResumenViewModel>();
        }

        // ViewModel para cada SO (Student Outcome)
        public class SOResumenViewModel
        {
            public string SO { get; set; }
            public int CantidadTotalEstudiantes { get; set; }
            public List<PIResumenViewModel> PIs { get; set; } = new List<PIResumenViewModel>();
        }

        // ViewModel para cada PI (Performance Indicator)
        public class PIResumenViewModel
        {
            public string PI { get; set; }
            public int CantidadTotalEstudiantes { get; set; }
            public int CantExperto { get; set; }
            public int CantSatisfactorio { get; set; }
            public int CantPrincipiante { get; set; }
            public int CantDesarrollo { get; set; }
            public decimal PorcentajeExperto { get; set; }
            public decimal PorcentajeSatisfactorio { get; set; }
            public decimal PorcentajePrincipiante { get; set; }
            public decimal PorcentajeDesarrollo { get; set; }
        }
    }
}
