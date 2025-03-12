using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Presentation.DTOs.ConfiguracionDTOs;
using AvaluoAPI.Presentation.ViewModels.CofiguracionViewModels;
using Humanizer;
using MapsterMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AvaluoAPI.Domain.Services.ConfiguracionService
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ConfiguracionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<ConfiguracionViewModel>> GetAll(string? descripcion, DateTime? fechaInicio, DateTime? fechaCierre, int? idEstado, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Configuraciones.GetConfiguraciones(descripcion, fechaInicio, fechaCierre, idEstado, page, recordsPerPage);
        }

        public async Task<ConfiguracionViewModel> GetById(int id)
        {
            var configuracion = await _unitOfWork.Configuraciones.GetConfiguracionById(id);
            if (configuracion == null)
                throw new KeyNotFoundException("Configuraion no encontrada.");

            return configuracion;
        }

        public async Task<FechaConfiguracionViewModel> GetFechas()
        {
            var fecha = await _unitOfWork.Configuraciones.GetFechaConfiguracion();
            if (fecha == null)
                throw new KeyNotFoundException("No hay una configuracion activa.");

            return fecha;
        }

        public async Task Register(ConfiguracionDTO configuracionDTO)
        {
            var resultado = DateTime.Compare(configuracionDTO.FechaInicio, configuracionDTO.FechaCierre);
            var (fechaInicioActual, _) = PeriodoExtensions.GetFechasTrimestreActual();
            var diaInicio = configuracionDTO.FechaInicio.DayOfYear;
            var diaCierre = configuracionDTO.FechaCierre.DayOfYear;
            var semana10 = fechaInicioActual.AddDays(63).DayOfYear;
            var semana12 = fechaInicioActual.AddDays(84).DayOfYear;
            var diferenciaDeFecha = diaCierre - diaInicio;

            var configuraciones = await _unitOfWork.Configuraciones.GetAllAsync();
            var estado = await _unitOfWork.Estados.GetEstadoByTablaName("Configuracion", "Activa");

            foreach (var config in configuraciones)
            {
                if (config.IdEstado == estado.Id && configuracionDTO.IdEstado == estado.Id)
                    throw new Exception("Ya hay una configuracion activa.");
            }

            if (resultado > 0)
                throw new Exception("La fecha de inicio no puede ser despues que la de cierre.");
            if (resultado == 0)
                throw new Exception("La fecha de inicio no puede ser la misma que la de cierre.");
            if (diferenciaDeFecha < 7)
                throw new Exception("La fecha de cierre debe de ser mas de 7 dias.");
            if (diaInicio < semana10) 
                throw new Exception("La fecha de inicio debe ser despues de la semana 10.");
            if (diaCierre > semana12)
                throw new Exception("La fecha de cierre debe ser antes de la semana 12.");

            await _unitOfWork.Configuraciones.AddAsync(new ConfiguracionEvaluaciones
            {
                Descripcion = configuracionDTO.Descripcion,
                IdEstado = configuracionDTO.IdEstado,
                FechaInicio = configuracionDTO.FechaInicio,
                FechaCierre = configuracionDTO.FechaCierre
            });
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, ConfiguracionDTO configuracionDTO)
        {
            var configuracion = await _unitOfWork.Configuraciones.GetConfiguracionById(id);
            if (configuracion == null)
                throw new KeyNotFoundException("La configuracion especificada no existe.");

            var estado = await _unitOfWork.Estados.GetByIdAsync(configuracionDTO.IdEstado);
            if (estado == null)
                throw new KeyNotFoundException("El estado especificado no existe.");

            var result = new ConfiguracionEvaluaciones {
                                Id = id,
                                Descripcion = configuracionDTO.Descripcion,
                                IdEstado = configuracionDTO.IdEstado,
                                FechaInicio = configuracionDTO.FechaInicio,
                                FechaCierre = configuracionDTO.FechaCierre
                            };

            await _unitOfWork.Configuraciones.Update(result);
            _unitOfWork.SaveChanges();
        }
    }
}
