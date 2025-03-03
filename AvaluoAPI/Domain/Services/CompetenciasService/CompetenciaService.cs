using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Services.EstadoService;
using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Domain.Services.CompetenciasService
{
    public class CompetenciaService : ICompetenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompetenciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CompetenciaViewModel> GetById(int id)
        {
            var competencia = await _unitOfWork.Competencias.GetCompetenciaById(id);
            if (competencia == null)
                throw new KeyNotFoundException("Competencia no encontrada.");

            return competencia;
        }


        public async Task<PaginatedResult<CompetenciaViewModel>> GetAll( string? nombre, string? acron, string? titulo, int? idTipo, int? idEstado, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Competencias.GetCompetencias(nombre, acron, titulo, idTipo, idEstado, page, recordsPerPage);
        }

        public async Task Register(CompetenciaDTO competenciaDTO)
        {
            var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(competenciaDTO.IdTipo);
            if (tipoCompetencia == null)
                throw new KeyNotFoundException("El Tipo de Competencia especificado no existe.");

            var estadoPorDefecto = await _unitOfWork.Estados.GetEstadoByTablaName("Competencia", "Activa");
            if (estadoPorDefecto == null)
                throw new KeyNotFoundException("No se encontró un estado por defecto para 'Competencia'.");

            var competencia = _mapper.Map<Competencia>(competenciaDTO);
            competencia.UltimaEdicion = DateTime.Now;
            competencia.IdEstado = estadoPorDefecto.Id;

            await _unitOfWork.Competencias.AddAsync(competencia);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, CompetenciaModifyDTO competenciaDTO)
        {
            var competencia = await _unitOfWork.Competencias.GetByIdAsync(id);
            if (competencia == null)
                throw new KeyNotFoundException("Competencia no encontrada.");

            var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(competenciaDTO.IdTipo);
            if (tipoCompetencia == null)
                throw new KeyNotFoundException("El Tipo de Competencia especificado no existe.");

            var estado = await _unitOfWork.Estados.GetByIdAsync(competenciaDTO.IdEstado);
            if (estado == null)
                throw new KeyNotFoundException("El Estado especificado no existe.");

            if (estado.IdTabla != "Competencia")
                throw new ValidationException("El Estado especificado no pertenece a las competencias.");

            _mapper.Map(competenciaDTO, competencia);
              competencia.UltimaEdicion = DateTime.UtcNow;

            await _unitOfWork.Competencias.Update(competencia);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var competencia = await _unitOfWork.Competencias.GetByIdAsync(id);
            if (competencia == null)
                throw new KeyNotFoundException("Competencia no encontrada.");

            _unitOfWork.Competencias.Delete(competencia);
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<AsignaturaConCompetenciasConEstadosViewModel>> GetMapaCompetencias(int idCarrera, int idTipoCompetencia)
        {
            return await _unitOfWork.Competencias.GetMapaCompetenciasWithEstados(idCarrera, idTipoCompetencia);
        }

        public async Task<bool> UpdateEstadoMapaCompetencia(int idAsignatura, int idCompetencia, UpdateEstadoMapaCompetenciaDTO dto)
        {
            var asignatura = await _unitOfWork.Asignaturas.GetByIdAsync(idAsignatura);
            if (asignatura == null)
                throw new KeyNotFoundException("La asignatura especificada no existe.");

            var competencia = await _unitOfWork.Competencias.GetByIdAsync(idCompetencia);
            if (competencia == null)
                throw new KeyNotFoundException("La competencia especificada no existe.");

            var estado = await _unitOfWork.Estados.GetByIdAsync(dto.IdEstado);
            if (estado == null || estado.IdTabla != "MapaCompetencia")
                throw new ValidationException("El estado especificado no es válido para Mapa_Competencias.");

            return await _unitOfWork.Competencias.UpdateEstadoMapaCompetencia(idAsignatura, idCompetencia, dto.IdEstado);
        }
    }
}
