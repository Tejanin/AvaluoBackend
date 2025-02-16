using AvaluoAPI.Presentation.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace AvaluoAPI.Swagger.Usuarios
{
    public class UsuarioByIdExampleProvider : IExamplesProvider<UsuarioViewModel>
    {
        public UsuarioViewModel GetExamples()
        {
            return new UsuarioViewModel
            {
                Id = 5,
                Username = "carlosmedina42",
                Email = "carlosmedina42@gmail.com",
                Nombre = "Carlos",
                Apellido = "Medina",
                Estado = "Activo",
                Area = "Área de Ingeniería",
                Rol = "Profesor",
                SO = "N/A",
                CV = "cv_carlosmedina.pdf",
                Foto = "foto_carlosmedina.jpg",
                Contactos = new List<ContactoViewModel>
            {
                new ContactoViewModel
                {
                    Id = 1,
                    NumeroContacto = "8095557788"
                },
                new ContactoViewModel
                {
                    Id = 2,
                    NumeroContacto = "8294569988"
                }
            }
            };
        }
    }
}
