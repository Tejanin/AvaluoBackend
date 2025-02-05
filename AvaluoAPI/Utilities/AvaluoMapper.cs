using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
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
                

            // Agregar Mapster a la inyección de dependencias
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();
        }
    }
}
