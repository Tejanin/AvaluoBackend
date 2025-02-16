using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Services.CompetenciasService;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<CompetenciaViewModel>> GetAll( string? nombre , string? acron , string? titulo , int? idTipo , int? idEstado )
            {
            IEnumerable<CompetenciaViewModel> competencias;
            if (string.IsNullOrEmpty(nombre) && string.IsNullOrEmpty(acron) && string.IsNullOrEmpty(titulo) && idTipo == null && idEstado == null)
            {

                competencias = await _unitOfWork.Competencias.GetAllCompetencias();
            } else
            {
                competencias = await _unitOfWork.Competencias.GetCompetenciasByFilter(nombre, acron, titulo, idTipo, idEstado);
            }
            if (!competencias.Any())
            {
                throw new KeyNotFoundException("No se encontraron competencias con los filtros especificados.");
            }
            return competencias;
        }


        public async Task Register(CompetenciaDTO competenciaDTO)
        {
            var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(competenciaDTO.IdTipo);
            if (tipoCompetencia == null)
                throw new KeyNotFoundException("El Tipo de Competencia especificado no existe.");

            var estado = await _unitOfWork.Estados.GetByIdAsync(competenciaDTO.IdEstado);
            if (estado == null)
                throw new KeyNotFoundException("El Estado especificado no existe.");

            var competencia = _mapper.Map<Competencia>(competenciaDTO);
            competencia.UltimaEdicion = DateTime.Now;

            await _unitOfWork.Competencias.AddAsync(competencia);
            _unitOfWork.SaveChanges();
        }


        public async Task Update(int id, CompetenciaDTO competenciaDTO)
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
    }
}
