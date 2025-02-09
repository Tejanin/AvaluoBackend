using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.TipoInformeService
{
    public class TipoInformeService : ITipoInformeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoInformeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TipoInformeViewModel> GetById(int id)
        {
            var tipoInforme = await _unitOfWork.TiposInformes.GetByIdAsync(id);
            if (tipoInforme == null)
                throw new KeyNotFoundException("Tipo de Informe no encontrado.");

            return _mapper.Map<TipoInformeViewModel>(tipoInforme);
        }

        public async Task<IEnumerable<TipoInformeViewModel>> GetAll()
        {
            var tiposInformes = await _unitOfWork.TiposInformes.GetAllAsync();
            return _mapper.Map<IEnumerable<TipoInformeViewModel>>(tiposInformes);
        }

        public async Task Register(TipoInformeDTO tipoInformeDTO)
        {
            var tipoInforme = _mapper.Map<TipoInforme>(tipoInformeDTO);
            await _unitOfWork.TiposInformes.AddAsync(tipoInforme);
            _unitOfWork.SaveChanges(); 
        }

        public async Task Update(int id, TipoInformeDTO tipoInformeDTO)
        {
            var tipoInforme = await _unitOfWork.TiposInformes.GetByIdAsync(id);
            if (tipoInforme == null)
                throw new KeyNotFoundException("Tipo de Informe no encontrado.");

            tipoInforme.Descripcion = tipoInformeDTO.Descripcion;
            await _unitOfWork.TiposInformes.Update(tipoInforme);
            _unitOfWork.SaveChanges(); 
        }

        public async Task Delete(int id)
        {
            var tipoInforme = await _unitOfWork.TiposInformes.GetByIdAsync(id);
            if (tipoInforme == null)
                throw new KeyNotFoundException("Tipo de Informe no encontrado.");

            _unitOfWork.TiposInformes.Delete(tipoInforme);
            _unitOfWork.SaveChanges(); 
        }
    }
}
