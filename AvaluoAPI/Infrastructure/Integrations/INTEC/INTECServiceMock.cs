using AvaluoAPI.Infrastructure.Integrations.INTEC.Models;

namespace AvaluoAPI.Infrastructure.Integrations.INTEC
{
    public interface IintecService
    {
        Task<List<SeccionModel>> GetSeccionesByProfesor(string id);
        Task<List<ProfesorModel>> GetProfesores();
    }
    public class INTECServiceMock : IintecService
    {
        private readonly List<ProfesorModel> _profesores;
        private readonly List<SeccionModel> _secciones;

        public INTECServiceMock()
        {
            // Inicializar datos mock
            _secciones = new List<SeccionModel>
        {
            new SeccionModel
            {
                Numero = "001",
                Asignatura = "Programación I",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088001", Nombre = "Juan", Apellido = "Pérez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088002", Nombre = "María", Apellido = "González", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088003", Nombre = "Carlos", Apellido = "Rodríguez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088004", Nombre = "Ana", Apellido = "Martínez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088005", Nombre = "Luis", Apellido = "Sánchez", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "002",
                Asignatura = "Estructuras de Datos",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088006", Nombre = "Laura", Apellido = "Díaz", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088007", Nombre = "Pedro", Apellido = "López", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088008", Nombre = "Sofia", Apellido = "Torres", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088009", Nombre = "Miguel", Apellido = "Ramírez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088010", Nombre = "Isabella", Apellido = "Morales", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "003",
                Asignatura = "Base de Datos",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088011", Nombre = "Diego", Apellido = "Fernández", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088012", Nombre = "Valentina", Apellido = "Castro", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088013", Nombre = "Andrés", Apellido = "Herrera", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088014", Nombre = "Camila", Apellido = "Vargas", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088015", Nombre = "Gabriel", Apellido = "Silva", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "004",
                Asignatura = "Algoritmos Avanzados",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088016", Nombre = "Julia", Apellido = "Mendoza", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088017", Nombre = "Mateo", Apellido = "Ríos", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088018", Nombre = "Daniela", Apellido = "Ortiz", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088019", Nombre = "Samuel", Apellido = "Cruz", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088020", Nombre = "Victoria", Apellido = "Reyes", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "005",
                Asignatura = "Desarrollo Web",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088021", Nombre = "Leonardo", Apellido = "Navarro", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088022", Nombre = "Valeria", Apellido = "Acosta", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088023", Nombre = "Alejandro", Apellido = "Medina", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088024", Nombre = "Mariana", Apellido = "Flores", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088025", Nombre = "Eduardo", Apellido = "Rojas", Cargo = "Estudiante" }
                }
            }
        };

            _profesores = new List<ProfesorModel>
        {
            new ProfesorModel
            {
                Id = "P001",
                Nombre = "Roberto",
                Apellido = "Martínez",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[0], _secciones[1] }
            },
            new ProfesorModel
            {
                Id = "P002",
                Nombre = "Carmen",
                Apellido = "Vásquez",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[2], _secciones[3] }
            },
            new ProfesorModel
            {
                Id = "P003",
                Nombre = "José",
                Apellido = "García",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[4] }
            }
        };
        }

        public async Task<List<ProfesorModel>> GetProfesores()
        {
            await Task.Delay(100);
            return _profesores;
        }

        public async Task<List<SeccionModel>> GetSeccionesByProfesor(string id)
        {
            await Task.Delay(100);

            var profesor = _profesores.FirstOrDefault(p => p.Id == id);
            if (profesor == null)
            {
                throw new KeyNotFoundException($"No se encontró el profesor con ID {id}");
            }

            return profesor.Secciones ?? new List<SeccionModel>();
        }
    }
}
