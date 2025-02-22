﻿using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.CarreraDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // Obtener todas las carreras con filtros y paginación
        public async Task<PaginatedResult<CarreraViewModel>> GetAll(
            string? nombreCarrera,
            int? idEstado,
            int? idArea,
            int? idCoordinadorCarrera,
            int? page,
            int? recordsPerPage)
        {
            return await _unitOfWork.Carreras.GetCarreras(nombreCarrera, idEstado, idArea, idCoordinadorCarrera, page, recordsPerPage);
        }

        // Obtener una carrera por ID con su Estado y Área
        public async Task<CarreraViewModel> GetById(int id)
        {
            var carrera = await _unitOfWork.Carreras.GetCarreraById(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            return carrera;
        }

        public async Task<IEnumerable<AsignaturaCarreraViewModel>> GetSubjectByCareer(int? idCarrera)
        {
            var asignaturasCarreras = await _unitOfWork.AsignaturasCarreras.GetAllByCareer(idCarrera);
            return _mapper.Map<IEnumerable<AsignaturaCarreraViewModel>>(asignaturasCarreras);
        }

        // Registrar una nueva carrera
        public async Task Register(CarreraDTO carreraDTO)
        {
            // Validar que el área exista
            var area = await _unitOfWork.Areas.GetByIdAsync(carreraDTO.IdArea);
            if (area == null)
                throw new KeyNotFoundException("El área especificada no existe.");

            // Obtener el estado por defecto
            var estadoPorDefecto = await _unitOfWork.Estados.GetEstadoByTablaName("Carrera", "Activa");
            if (estadoPorDefecto == null)
                throw new KeyNotFoundException("No se encontró un estado por defecto para 'Carrera'.");

            // Validar que el coordinador exista
            var coordinador = await _unitOfWork.Usuarios.GetUsuarioWithRolById(carreraDTO.IdCoordinadorCarrera);
            if (coordinador == null)
                throw new KeyNotFoundException("El usuario especificado no existe.");

            if(coordinador?.Rol?.EsCoordinadorCarrera != true) 
                throw new KeyNotFoundException("El usuario especificado no tiene el rol de Coordinador de Carrera");

            // Mapear DTO a Entidad
            var carrera = _mapper.Map<Carrera>(carreraDTO);
            carrera.UltimaEdicion = DateTime.UtcNow;
            carrera.IdEstado = estadoPorDefecto.Id;

            // Guardar en la base de datos
            await _unitOfWork.Carreras.AddAsync(carrera);
            _unitOfWork.SaveChanges();
        }

        // Actualizar una carrera existente
        public async Task Update(int id, CarreraModifyDTO carreraDTO)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            // Validar que el área exista
            var area = await _unitOfWork.Areas.GetByIdAsync(carreraDTO.IdArea);
            if (area == null)
                throw new KeyNotFoundException("El área especificada no existe.");

            // Validar que el estado exista y pertenezca a 'Carrera'
            var estado = await _unitOfWork.Estados.GetByIdAsync(carreraDTO.IdEstado);
            if (estado == null)
                throw new KeyNotFoundException("El estado especificado no existe.");
            if (estado.IdTabla != "Carrera")
                throw new ValidationException("El estado especificado no pertenece a 'Carrera'.");

            // Validar que el coordinador exista
            var coordinador = await _unitOfWork.Usuarios.GetUsuarioWithRolById(carreraDTO.IdCoordinadorCarrera);
            if (coordinador == null)
                throw new KeyNotFoundException("El usuario especificado no existe.");

            if (coordinador?.Rol?.EsCoordinadorCarrera != true)
                throw new KeyNotFoundException("El usuario especificado no tiene el rol de Coordinador de Carrera");

            // Mapear DTO a Entidad
            _mapper.Map(carreraDTO, carrera);
            carrera.UltimaEdicion = DateTime.UtcNow;

            // Guardar cambios en la base de datos
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


        // Eliminar una carrera
        public async Task Delete(int id)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(id);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera no encontrada.");

            _unitOfWork.Carreras.Delete(carrera);
            _unitOfWork.SaveChanges();
        }
    }
}
