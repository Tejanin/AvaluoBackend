using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs;
using AvaluoAPI.Presentation.DTOs.TipoCompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System;
using System.Collections.Generic;
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
                throw new Exception("Tipo de competencia no encontrado.");

            return _mapper.Map<TipoCompetenciaViewModel>(tipoCompetencia);
        }

        public async Task<IEnumerable<TipoCompetenciaViewModel>> GetAll()
        {
            var tiposCompetencias = await _unitOfWork.TiposCompetencias.GetAllAsync();
            return _mapper.Map<IEnumerable<TipoCompetenciaViewModel>>(tiposCompetencias);
        }

        public async Task Register(TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var tipoCompetencia = _mapper.Map<TipoCompetencia>(tipoCompetenciaDTO);
                tipoCompetencia.FechaCreacion = DateTime.Now;
                tipoCompetencia.UltimaEdicion = DateTime.Now;

                await _unitOfWork.TiposCompetencias.AddAsync(tipoCompetencia);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task Update(int id, TipoCompetenciaDTO tipoCompetenciaDTO)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(id);
                if (tipoCompetencia == null)
                    throw new Exception("Tipo de competencia no encontrado.");

                tipoCompetencia.Nombre = tipoCompetenciaDTO.Nombre;
                tipoCompetencia.UltimaEdicion = DateTime.Now;

                await _unitOfWork.TiposCompetencias.Update(tipoCompetencia);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var tipoCompetencia = await _unitOfWork.TiposCompetencias.GetByIdAsync(id);
                if (tipoCompetencia == null)
                    throw new Exception("Tipo de competencia no encontrado.");

                _unitOfWork.TiposCompetencias.Delete(tipoCompetencia);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
