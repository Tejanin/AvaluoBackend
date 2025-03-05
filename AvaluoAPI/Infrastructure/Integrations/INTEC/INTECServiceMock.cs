using AvaluoAPI.Infrastructure.Integrations.INTEC.Models;

namespace AvaluoAPI.Infrastructure.Integrations.INTEC
{
    public interface IintecService
    {
        Task<List<SeccionModel>> GetSeccionesByProfesor(string id);
        Task<List<SeccionModel>> GetSecciones();
        Task<List<ProfesorModel>> GetProfesores(string seccion = null, string asignatura = null);
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
                Asignatura = "IDS340 - DESARROLLO DE SOFTWARE I",
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
                Asignatura = "IDS343L - LABORATORIO ESTRUCTURAS DE DATOS Y ALGORITMOS I",
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
                Asignatura = "IDS323",
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
                Asignatura = "IDS341 - DESARROLLO DE SOFTWARE II",
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
                Asignatura = "IDS344",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088021", Nombre = "Leonardo", Apellido = "Navarro", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088022", Nombre = "Valeria", Apellido = "Acosta", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088023", Nombre = "Alejandro", Apellido = "Medina", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088024", Nombre = "Mariana", Apellido = "Flores", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088025", Nombre = "Eduardo", Apellido = "Rojas", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "006",
                Asignatura = "IDS345L",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088026", Nombre = "Emilio", Apellido = "Guzmán", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088027", Nombre = "Regina", Apellido = "Palacios", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088028", Nombre = "Fernando", Apellido = "Quintero", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088029", Nombre = "Carolina", Apellido = "Jiménez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088030", Nombre = "Ricardo", Apellido = "Beltrán", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "007",
                Asignatura = "IDS345L",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088031", Nombre = "Isabel", Apellido = "Ochoa", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088032", Nombre = "Héctor", Apellido = "Velázquez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088033", Nombre = "Lucía", Apellido = "Delgado", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088034", Nombre = "Roberto", Apellido = "Zamora", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088035", Nombre = "Patricia", Apellido = "Molina", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "008",
                Asignatura = "IDS345",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088036", Nombre = "Gustavo", Apellido = "Aguirre", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088037", Nombre = "Natalia", Apellido = "Espinoza", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088038", Nombre = "Francisco", Apellido = "Luna", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088039", Nombre = "Andrea", Apellido = "Cordero", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088040", Nombre = "Javier", Apellido = "Soto", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "009",
                Asignatura = "INS377L - LABORATORIO BASES DE DATOS I",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088041", Nombre = "Marina", Apellido = "Rivas", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088042", Nombre = "Antonio", Apellido = "Cabrera", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088043", Nombre = "Cristina", Apellido = "Durán", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088044", Nombre = "Raúl", Apellido = "Campos", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088045", Nombre = "Adriana", Apellido = "Mora", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "010",
                Asignatura = "INS377 - BASES DE DATOS I",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088046", Nombre = "Pablo", Apellido = "Guerra", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088047", Nombre = "Diana", Apellido = "Santos", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088048", Nombre = "Óscar", Apellido = "Valencia", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088049", Nombre = "Mónica", Apellido = "Paredes", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088050", Nombre = "Jorge", Apellido = "Cervantes", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "011",
                Asignatura = "IDS324",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088051", Nombre = "Beatriz", Apellido = "Barrios", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088052", Nombre = "Manuel", Apellido = "Contreras", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088053", Nombre = "Cecilia", Apellido = "Méndez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088054", Nombre = "Hugo", Apellido = "Zavala", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088055", Nombre = "Elena", Apellido = "Fuentes", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "012",
                Asignatura = "IDS208 - TEAM BUILDING",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088056", Nombre = "Arturo", Apellido = "Ibarra", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088057", Nombre = "Silvia", Apellido = "Arenas", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088058", Nombre = "Ignacio", Apellido = "Ponce", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088059", Nombre = "Rosa", Apellido = "Villanueva", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088060", Nombre = "Felipe", Apellido = "Escobar", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "013",
                Asignatura = "IDS208 - TEAM BUILDING",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088061", Nombre = "Carmen", Apellido = "Valdez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088062", Nombre = "Rodrigo", Apellido = "Núñez", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088063", Nombre = "Lorena", Apellido = "Estrada", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088064", Nombre = "Alberto", Apellido = "Salazar", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088065", Nombre = "Martha", Apellido = "Varela", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "014",
                Asignatura = "IDS311 - PROCESO DE SOFTWARE",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088066", Nombre = "Vicente", Apellido = "Cárdenas", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088067", Nombre = "Teresa", Apellido = "Pacheco", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088068", Nombre = "Marcos", Apellido = "Vega", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088069", Nombre = "Gloria", Apellido = "Miranda", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088070", Nombre = "Salvador", Apellido = "Maldonado", Cargo = "Estudiante" }
                }
            },
            new SeccionModel
            {
                Numero = "015",
                Asignatura = "IDS311 - PROCESO DE SOFTWARE",
                Estudiantes = new List<PersonModel>
                {
                    new EstudianteModel { Id = "1088071", Nombre = "Alicia", Apellido = "Rosales", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088072", Nombre = "Ramón", Apellido = "Carrillo", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088073", Nombre = "Claudia", Apellido = "Montes", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088074", Nombre = "Ernesto", Apellido = "Bravo", Cargo = "Estudiante" },
                    new EstudianteModel { Id = "1088075", Nombre = "Rocío", Apellido = "Pineda", Cargo = "Estudiante" }
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
                Email = "rmartinez@gmail.com",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[0], _secciones[1], _secciones[8], _secciones[9] }  // IDS340, IDS343, INS377L, INS377
            },
            new ProfesorModel
            {
                Id = "P002",
                Nombre = "Carmen",
                Apellido = "Vásquez",
                Email = "cvaquez@gmail.com",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[2], _secciones[3], _secciones[10] }  // IDS323, IDS341, IDS324
            },
            new ProfesorModel
            {
                Id = "P003",
                Nombre = "José",
                Apellido = "García",
                Email = "jgarcia@gmail.com",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[4], _secciones[11], _secciones[12] }  // IDS344, IDS208 (ambas secciones)
            },
            new ProfesorModel
            {
                Id = "P004",
                Nombre = "Ana",
                Apellido = "Ramírez",
                Email = "aramirez@gmail.com",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[5], _secciones[6] }  // IDS345L (ambas secciones)
            },
            new ProfesorModel
            {
                Id = "P005",
                Nombre = "Miguel",
                Apellido = "Torres",
                Email = "mtorres@gmail.com",
                Cargo = "Profesor",
                Secciones = new List<SeccionModel> { _secciones[7], _secciones[13], _secciones[14] }  // IDS345, IDS311 (ambas secciones)
            }
                    };
        }

        public async Task<List<ProfesorModel>> GetProfesores(string? seccion, string? asignatura )
        {
            return await Task.Run(() =>
            {
                // Crear una lista para almacenar los resultados
                var profesoresFiltrados = new List<ProfesorModel>();

                // Recorrer todos los profesores
                foreach (var profesor in _profesores)
                {
                    // Obtener las secciones que coinciden con los criterios de filtrado
                    var seccionesFiltradas = new List<SeccionModel>();

                    // Recorrer las secciones del profesor y aplicar filtros
                    foreach (var seccionProf in profesor.Secciones)
                    {
                        bool coincideSeccion = string.IsNullOrEmpty(seccion) || seccionProf.Numero == seccion;
                        bool coincideAsignatura = string.IsNullOrEmpty(asignatura) || seccionProf.Asignatura == asignatura;

                        // Si ambos filtros coinciden (o no se han especificado), añadir la sección
                        if (coincideSeccion && coincideAsignatura)
                        {
                            seccionesFiltradas.Add(seccionProf);
                        }
                    }

                    // Si el profesor tiene secciones que cumplen con los criterios
                    if (seccionesFiltradas.Any())
                    {
                        // Crear una copia del profesor
                        var profesorFiltrado = new ProfesorModel
                        {
                            Id = profesor.Id,
                            Nombre = profesor.Nombre,
                            Apellido = profesor.Apellido,
                            Email = profesor.Email,
                            Cargo = profesor.Cargo,
                            // Asignar solo las secciones filtradas
                            Secciones = seccionesFiltradas
                        };

                        profesoresFiltrados.Add(profesorFiltrado);
                    }
                }

                return profesoresFiltrados;
            });
        }

        public Task<List<SeccionModel>> GetSecciones()
        {
            return Task.FromResult(_secciones);
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
