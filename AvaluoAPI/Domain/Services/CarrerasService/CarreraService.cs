using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.CarreraDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;

namespace AvaluoAPI.Domain.Services.CarreraService
{
    public class CarreraService : ICarreraService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarreraService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CarreraViewModel>> GetAll(
            string? nombreCarrera,
            int? idEstado,
            int? idArea,
            int? idCoordinadorCarrera,
            int? año, 
            string? peos,
            int? page,
            int? recordsPerPage)
        {
            return await _unitOfWork.Carreras.GetCarreras(nombreCarrera, idEstado, idArea, idCoordinadorCarrera, año, peos, page, recordsPerPage);
        }

        public async Task<CarreraViewModel> GetById(int id)
        {
            var carrera = await _unitOfWork.Carreras.GetCarreraById(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            return carrera;
        }

        public async Task Register(CarreraDTO carreraDTO)
        {
            var area = await _unitOfWork.Areas.GetByIdAsync(carreraDTO.IdArea);
            if (area == null)
                throw new KeyNotFoundException("El área especificada no existe.");

            var estadoPorDefecto = await _unitOfWork.Estados.GetEstadoByTablaName("Carrera", "Activa");
            if (estadoPorDefecto == null)
                throw new KeyNotFoundException("No se encontró un estado por defecto para 'Carrera'.");


            if (carreraDTO.IdCoordinadorCarrera.HasValue)
            {
                var coordinador = await _unitOfWork.Usuarios.GetUsuarioWithRolById(carreraDTO.IdCoordinadorCarrera.Value);
                if (coordinador == null)
                    throw new KeyNotFoundException("El usuario especificado no existe.");
                if (coordinador?.Rol?.EsCoordinadorCarrera != true)
                    throw new KeyNotFoundException("El usuario especificado no tiene el rol de Coordinador de Carrera");
            }







            var carrera = _mapper.Map<Carrera>(carreraDTO);
            carrera.UltimaEdicion = DateTime.UtcNow;
            carrera.IdEstado = estadoPorDefecto.Id;

            await _unitOfWork.Carreras.AddAsync(carrera);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, CarreraModifyDTO carreraDTO)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            var area = await _unitOfWork.Areas.GetByIdAsync(carreraDTO.IdArea);
            if (area == null)
                throw new KeyNotFoundException("El área especificada no existe.");

            

            var coordinador = await _unitOfWork.Usuarios.GetUsuarioWithRolById(carreraDTO.IdCoordinadorCarrera);
            if (coordinador == null)
                throw new KeyNotFoundException("El usuario especificado no existe.");

            if (coordinador?.Rol?.EsCoordinadorCarrera != true)
                throw new KeyNotFoundException("El usuario especificado no tiene el rol de Coordinador de Carrera");

            _mapper.Map(carreraDTO, carrera);
            carrera.UltimaEdicion = DateTime.UtcNow;

            await _unitOfWork.Carreras.Update(carrera);
            _unitOfWork.SaveChanges();
        }
        public async Task UpdatePEOs(int id, string nuevosPEOs)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            carrera.PEOs = nuevosPEOs;
            carrera.UltimaEdicion = DateTime.UtcNow;

            await _unitOfWork.Carreras.Update(carrera);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            _unitOfWork.Carreras.Delete(carrera);
            _unitOfWork.SaveChanges();
        }
        public async Task<IEnumerable<CarreraViewModel>> GetCarrerasByArea(int idArea)
        {
            return await _unitOfWork.Carreras.GetCarrerasByArea(idArea);
        }
    }
}
