using AvaluoAPI.Infrastructure.Integrations.Moodle.Models;

namespace AvaluoAPI.Infrastructure.Integrations.Moodle.Mock
{
    public class MoodleMock
    {
        private readonly List<string> _codigosAsignatura = new()
    {
        "IDS323", // Técnicas Fundamentales de Ingeniería de Software
        "IDS340", // Desarrollo de Software I
        "IDS341", // Desarrollo de Software II
        "IDS343", // Estructuras de Datos y Algoritmos I
        "IDS344"  // Estructuras de Datos y Algoritmos II
    };

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

        public  List<ListaDeEvidenciasMoodleModel> GetMockData()
        {
            var random = new Random();
            var listaEvidencias = new List<ListaDeEvidenciasMoodleModel>();

            foreach (var codigo in _codigosAsignatura)
            {
                var seccion = random.Next(1, 5).ToString("D2");

                var evidencias = new ListaDeEvidenciasMoodleModel
                {
                    CodigoAsignatura = codigo,
                    Seccion = seccion,
                    NombreDeEvidencias = GenerarEvidencias(random)
                };

                listaEvidencias.Add(evidencias);
            }

            return listaEvidencias;
        }

        private List<EvidenciasMoodleModel> GenerarEvidencias(Random random)
        {
            var cantidadEvidencias = random.Next(3, 6);
            var evidencias = new List<EvidenciasMoodleModel>();

            for (int i = 0; i < cantidadEvidencias; i++)
            {
                var evidencia = new EvidenciasMoodleModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = _nombresTareas[random.Next(_nombresTareas.Count)],
                   
                    Estudiantes = GenerarEstudiantes(random)
                };

                evidencias.Add(evidencia);
            }

            return evidencias;
        }

        private List<EstudianteMoodleModel> GenerarEstudiantes(Random random)
        {
            var cantidadEstudiantes = random.Next(15, 31);
            var estudiantes = new List<EstudianteMoodleModel>();

            for (int i = 0; i < cantidadEstudiantes; i++)
            {
                var estudiante = new EstudianteMoodleModel
                {
                    Matricula = GenerarMatricula(random),
                    Nombre = GenerarNombre(random),
                    Apellido = GenerarApellido(random),
                    Ruta = $"/moodle/evidencias/{Guid.NewGuid():N}"  // Movimos la ruta aquí
                };

                estudiantes.Add(estudiante);
            }

            return estudiantes;
        }

        private string GenerarMatricula(Random random)
        {
            return random.Next(1000000, 1200000).ToString();
        }

        private string GenerarNombre(Random random)
        {
            var nombres = new[] { "Juan", "María", "Carlos", "Ana", "Luis", "Laura", "Miguel", "Sofia", "José", "Isabella" };
            return nombres[random.Next(nombres.Length)];
        }

        private string GenerarApellido(Random random)
        {
            var apellidos = new[] { "García", "Rodríguez", "Martínez", "López", "Pérez", "González", "Sánchez", "Ramírez" };
            return apellidos[random.Next(apellidos.Length)];
        }
    }
}
