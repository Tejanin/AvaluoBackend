using AvaluoAPI.Domain;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using System.Text.Json.Serialization;

namespace AvaluoAPI.Presentation.ViewModels
{
    public class ResumenBase
    {
        
        public int IdPI { get; set; }

        
    }

    public class ResumenViewModelWithEstudiantes : ResumenBase
    {
        public List<EstudianteDTO> Estudiantes { get; set; }
    }

    public class  ResumenViewModel : ResumenBase
    {
        public int CantExperto { get; set; }
        public int CantSatisfactorio { get; set; }
        public int CantPrincipiante { get; set; }
        public int CantDesarrollo { get; set; }
    }

    public class ResumenViewModelMixed : ResumenBase
    {
        public int CantExperto { get; set; }
        public int CantSatisfactorio { get; set; }
        public int CantPrincipiante { get; set; }
        public int CantDesarrollo { get; set; }
        public List<EstudianteDTO>? Estudiantes { get; set; }
    }
}
