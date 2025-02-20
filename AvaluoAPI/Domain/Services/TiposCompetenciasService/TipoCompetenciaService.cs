using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.TipoCompetenciaService
{
    public class TipoCompetenciaService : ITipoCompetenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoCompetenciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TipoCompetenciaViewModel> GetById(int id)
        {
            var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(id);
            if (tipoCompetencia == null)
                throw new KeyNotFoundException("Tipo de competencia no encontrado.");

            return _mapper.Map<TipoCompetenciaViewModel>(tipoCompetencia);
        }

        public async Task<PaginatedResult<TipoCompetenciaViewModel>> GetAll(int? page, int? recordsPerPage)
        {
            var tiposQuery = _unitOfWork.TiposCompetencias.AsQueryable();
            var paginatedResult = await _unitOfWork.TiposCompetencias.PaginateWithQuery(tiposQuery, page, recordsPerPage);

            return paginatedResult.Convert(e => _mapper.Map<TipoCompetenciaViewModel>(e));
        }

        public async Task Register(TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            var tipoCompetencia = _mapper.Map<TipoCompetencia>(tipoCompetenciaDTO);
            tipoCompetencia.UltimaEdicion = DateTime.Now;

            await _unitOfWork.TiposCompetencias.AddAsync(tipoCompetencia);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(id);
            if (tipoCompetencia == null)
                throw new KeyNotFoundException("Tipo de competencia no encontrado.");

            tipoCompetencia.Nombre = tipoCompetenciaDTO.Nombre;
            tipoCompetencia.UltimaEdicion = DateTime.Now;

            await _unitOfWork.TiposCompetencias.Update(tipoCompetencia);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(id);
            if (tipoCompetencia == null)
                throw new KeyNotFoundException("Tipo de competencia no encontrado.");

            _unitOfWork.TiposCompetencias.Delete(tipoCompetencia);
            _unitOfWork.SaveChanges();
        }
    }
}
