using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasRepositories;
using AvaluoAPI.Presentation.DTOs.AsignaturaDTOs;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.AsignaturaService
{
    public class AsignaturaService : IAsignaturaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AsignaturaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<AsignaturaViewModel> GetById(int id)
        {
            var asignatura = await _unitOfWork.Asignaturas.GetAsignaturaById(id);
            if (asignatura == null)
                throw new KeyNotFoundException("Asignatura no encontrada.");

            return asignatura;
        }

        public async Task<PaginatedResult<AsignaturaViewModel>> GetAll(string? codigo, string? nombre, int? idEstado, int? idArea, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Asignaturas.GetAsignaturas(codigo, nombre, idEstado, idArea, page, recordsPerPage);
        }

        public async Task<PaginatedResult<AsignaturaViewModel>> GetSubjectByCareer(int idCarrera, int? page, int? recordsPerPage)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(idCarrera);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera especificada no encontrada.");

            return await _unitOfWork.AsignaturasCarreras.GetAllByCareer(idCarrera, page, recordsPerPage);
        }

        public async Task Register(AsignaturaDTO asignaturaDTO)
        {  
            var area = await _unitOfWork.Areas.GetByIdAsync(asignaturaDTO.IdArea);
            if (area == null)
                throw new KeyNotFoundException("El area especificada no existe.");

            var estadoPorDefecto = await _unitOfWork.Estados.GetEstadoByTablaName("Asignatura", "Activa");
            if (estadoPorDefecto == null)
                throw new KeyNotFoundException("No se encontró un estado por defecto para 'Asignatura'.");

            var asignatura = _mapper.Map<Asignatura>(asignaturaDTO);
            asignatura.UltimaEdicion = DateTime.Now;
            asignatura.IdEstado = estadoPorDefecto.Id;

            await _unitOfWork.Asignaturas.AddAsync(asignatura);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, AsignaturaModifyDTO asignaturaDTO)
        {
            var asignatura = await _unitOfWork.Asignaturas.GetByIdAsync(id);
            if (asignatura == null)
                throw new KeyNotFoundException("Asignatura no encontrada.");

            _mapper.Map(asignaturaDTO, asignatura);
            await _unitOfWork.Asignaturas.Update(asignatura);

            var area = await _unitOfWork.Areas.GetByIdAsync(asignaturaDTO.IdArea);
            if (area == null)
                throw new KeyNotFoundException("El area especificada no existe.");

            var estado = await _unitOfWork.Estados.GetByIdAsync(asignaturaDTO.IdEstado);
            if (estado == null)
                throw new KeyNotFoundException("El Estado especificado no existe.");

            if (estado.IdTabla != "Asignatura")
                throw new ValidationException("El Estado especificado no pertenece a las Asignaturas.");

            _mapper.Map(asignaturaDTO, asignatura);
            asignatura.UltimaEdicion = DateTime.UtcNow;

            await _unitOfWork.Asignaturas.Update(asignatura);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var asignatura = await _unitOfWork.Asignaturas.GetByIdAsync(id);
            if (asignatura == null)
                throw new KeyNotFoundException("Asignatura no encontrada.");

            _unitOfWork.Asignaturas.Delete(asignatura);
            _unitOfWork.SaveChanges();
        }
    }
}
