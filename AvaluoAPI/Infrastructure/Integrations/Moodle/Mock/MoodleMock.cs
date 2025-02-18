using AvaluoAPI.Infrastructure.Integrations.INTEC.Models;
using AvaluoAPI.Infrastructure.Integrations.INTEC;
using AvaluoAPI.Infrastructure.Integrations.Moodle.Models;

namespace AvaluoAPI.Infrastructure.Integrations.Moodle.Mock
{
    public class MoodleMock
    {
        private readonly Dictionary<string, List<PersonModel>> _seccionesEstudiantes;
        private readonly List<string> _nombresTareas = new()
    {
        "Proyecto Final",
        "Diseño de Sistema",
        "Implementación de Algoritmos",
        "Presentación de Arquitectura",
        "Tarea #1: Diagramas UML",
        "Tarea #2: Patrones de Diseño",
        "Evaluación de Código",
        "Documentación de API"
    };

        public MoodleMock()
        {
            var intecService = new INTECServiceMock();
            var secciones = intecService.GetSecciones().Result;
            _seccionesEstudiantes = secciones.ToDictionary(
                s => s.Asignatura,
                s => s.Estudiantes
            );
        }

        public List<ListaDeEvidenciasMoodleModel> GetMockData()
        {
            var listaEvidencias = new List<ListaDeEvidenciasMoodleModel>();

            foreach (var seccion in _seccionesEstudiantes)
            {
                var evidencias = new ListaDeEvidenciasMoodleModel
                {
                    CodigoAsignatura = seccion.Key,
                    Seccion = ObtenerSeccionPorAsignatura(seccion.Key),
                    NombreDeEvidencias = GenerarEvidencias(seccion.Value)
                };
                listaEvidencias.Add(evidencias);
            }

            return listaEvidencias;
        }

        private string ObtenerSeccionPorAsignatura(string codigoAsignatura)
        {
            var intecService = new INTECServiceMock();
            var seccion = intecService.GetSecciones().Result
                .First(s => s.Asignatura == codigoAsignatura);
            return seccion.Numero;
        }

        private List<EvidenciasMoodleModel> GenerarEvidencias(List<PersonModel> estudiantes)
        {
            var evidencias = new List<EvidenciasMoodleModel>();
            // Usar 4 tareas fijas para cada asignatura
            for (int i = 0; i < 4; i++)
            {
                var evidencia = new EvidenciasMoodleModel
                {
                    Id = $"EV{i + 1}",
                    Nombre = _nombresTareas[i],
                    Estudiantes = estudiantes.Select(e => new EstudianteMoodleModel
                    {
                        Matricula = ((EstudianteModel)e).Id,
                        Nombre = e.Nombre,
                        Apellido = e.Apellido,
                        Ruta = $"/moodle/evidencias/{((EstudianteModel)e).Id}/{i + 1}"
                    }).ToList()
                };
                evidencias.Add(evidencia);
            }
            return evidencias;
        }
    }
}
