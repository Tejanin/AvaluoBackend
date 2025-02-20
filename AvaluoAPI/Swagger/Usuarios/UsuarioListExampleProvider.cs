using AvaluoAPI.Presentation.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace AvaluoAPI.Swagger.Usuarios
{
    public class UsuariosListExampleProvider : IExamplesProvider<List<UsuarioViewModel>>
    {
        public List<UsuarioViewModel> GetExamples()
        {
            var usuarios = new List<UsuarioViewModel>();

            // Datos de ejemplo
            var nombres = new[] { "Juan", "María", "Pedro", "Ana", "Carlos", "Laura", "José", "Sofia", "Miguel", "Isabel" };
            var apellidos = new[] { "García", "Rodríguez", "Martínez", "López", "Pérez", "Fernández", "González", "Sánchez", "Ramírez", "Torres" };
            var areas = new[] { "Área de Sistemas", "Área de Ingeniería", "Área de Recursos Humanos", "Área de Finanzas", "Área de Marketing" };
            var roles = new[] { "Desarrollador", "Profesor", "Coordinador", "Analista", "Gerente", "Supervisor", "Técnico" };
            var estados = new[] { "Activo", "Inactivo" };

            var random = new Random();

            for (int i = 1; i <= 20; i++)
            {
                var nombre = nombres[random.Next(nombres.Length)];
                var apellido = apellidos[random.Next(apellidos.Length)];
                var username = $"{nombre.ToLower()}{apellido.ToLower()}{random.Next(10, 99)}";

                var usuario = new UsuarioViewModel
                {
                    Id = i,
                    Username = username,
                    Email = $"{username}@{(random.Next(2) == 0 ? "gmail.com" : "outlook.com")}",
                    Nombre = nombre,
                    Apellido = apellido,
                    Estado = estados[random.Next(estados.Length)],
                    Area = areas[random.Next(areas.Length)],
                    Rol = roles[random.Next(roles.Length)],
                    SO = "N/A",
                    CV = random.Next(2) == 0 ? $"cv_{username}.pdf" : null,
                    Foto = random.Next(2) == 0 ? $"foto_{username}.jpg" : null,
                    Contactos = GenerarContactos(random.Next(4))
                };

                usuarios.Add(usuario);
            }

            return usuarios;
        }

        private List<ContactoViewModel> GenerarContactos(int cantidad)
        {
            var contactos = new List<ContactoViewModel>();
            var random = new Random();

            for (int i = 1; i <= cantidad; i++)
            {
                contactos.Add(new ContactoViewModel
                {
                    Id = random.Next(1, 1000),
                    NumeroContacto = $"8{random.Next(0, 5)}9{random.Next(1000000, 9999999)}"
                });
            }

            return contactos;
        }
    }
}
