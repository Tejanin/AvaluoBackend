using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Domain;
using AvaluoAPI.Infrastructure.Integrations.INTEC.Models;
using AvaluoAPI.Presentation.DTOs.RubricaDTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using Mapster;
using MapsterMapper;
using System.Runtime;

namespace AvaluoAPI.Utilities
{
    public static class MappingConfig
    {
        public static void AddMappingConfiguration(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Configuración personalizada para Usuario -> UsuarioDto
            config.NewConfig<UsuarioDTO, Usuario>()
                .Map(dest => dest.HashedPassword, src => src.Password);


            // Configuración personalizada para Usuario -> UsuarioDto
           
            // Configuracion personalizada para PersonModel -> EstudianteDTO
            config.NewConfig<PersonModel, EstudianteDTO>()
                .Map(dest => dest.Nombre, src => src.Nombre)
                .Map(dest => dest.Apellido, src => src.Apellido)
                .Map(dest => dest.Matricula, src => src.Id);

            config.NewConfig<PI, PIViewModel>();

            config.NewConfig<ResumenDTO, ResumenViewModelMixed>()
                .Map(dest => dest.Estudiantes, src => src.Estudiantes)
                .Map(dest => dest.IdPI, src => src.IdPI);

            // Agregar Mapster a la inyección de dependencias
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }
    }
}
