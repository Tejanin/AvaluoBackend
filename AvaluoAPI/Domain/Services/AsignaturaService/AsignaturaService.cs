using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasRepositories;
using AvaluoAPI.Presentation.DTOs.AsignaturaCarreraDTOs;
using AvaluoAPI.Presentation.DTOs.AsignaturaDTOs;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
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
        private readonly FileHandler _fileHandler;

        public AsignaturaService(IUnitOfWork unitOfWork, IMapper mapper, FileHandler fileHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandler = fileHandler;
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

        public async Task RegisterSubjectByCareer(AsignaturaCarreraDTO asignaturaCarreraDTO)
        {
            var carrera = await _unitOfWork.Carreras.GetByIdAsync(asignaturaCarreraDTO.IdCarrera);
            if (carrera == null)
                throw new KeyNotFoundException("Carrera especificada no encontrada.");

            var asignatura = await _unitOfWork.Asignaturas.GetByIdAsync(asignaturaCarreraDTO.IdAsignatura);
            if (asignatura == null)
                    throw new KeyNotFoundException("Asignatura especificada no encontrada.");

            var existingRelation = await _unitOfWork.AsignaturasCarreras.GetByCarreraAsignaturaAsync(asignaturaCarreraDTO.IdCarrera, asignaturaCarreraDTO.IdAsignatura);
            if (existingRelation != null)
                throw new InvalidOperationException("Esta asignatura ya está asignada a esta carrera.");

            var asignaturaCarrera = _mapper.Map<AsignaturaCarrera>(asignaturaCarreraDTO);

            await _unitOfWork.AsignaturasCarreras.AddAsync(asignaturaCarrera);
            _unitOfWork.SaveChanges();
        }

        public async Task DeleteGetSubjectByCareer(AsignaturaCarreraDTO asignaturaCarreraDTO)
        {
            var asignaturaCarrera = await _unitOfWork.AsignaturasCarreras.GetByCarreraAsignaturaAsync(asignaturaCarreraDTO.IdCarrera, asignaturaCarreraDTO.IdAsignatura); ;
            if (asignaturaCarrera == null)
                throw new KeyNotFoundException("La asignatura y carrera no se fue encontrada relacionadas");

            _unitOfWork.AsignaturasCarreras.Delete(asignaturaCarrera);
            _unitOfWork.SaveChanges();
        }

        public async Task UpdateDocument(int id, IFormFile file, string tipoDocumento)
        {
            var asignatura = await _unitOfWork.Asignaturas.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Asignatura no encontrada.");

            // Validar y obtener sufijo según tipo de documento
            string sufijo = tipoDocumento switch
            {
                "Programa" => "Programa_Asignatura",
                "Syllabus" => "Syllabus",
                _ => throw new Exception("No se pudo identificar el documento que desea cargar")
            };

            // Guardar el archivo
            RutaAsignaturaBuilder rutaBuilder = new RutaAsignaturaBuilder(asignatura.Codigo, tipoDocumento);
            (bool exito, string mensaje, string ruta, _) = await _fileHandler.Upload(
                file,
                new List<string> { ".pdf" },
                rutaBuilder,
                nombre => $"{sufijo}_{asignatura.Codigo}"
            );

            if (!exito) throw new Exception(mensaje);

            // Actualizar la ruta según el tipo de documento
            if (tipoDocumento == "Programa")
            { 
                asignatura.ProgramaAsignatura = ruta;
            } else { 
                asignatura.Syllabus = ruta; 
            }

            await _unitOfWork.Asignaturas.Update(asignatura);
            _unitOfWork.SaveChanges();
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
