using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.EstadoDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.EstadoService
{
    public class EstadoService : IEstadoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EstadoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EstadoViewModel> GetById(int id)
        {
            var estado = await _unitOfWork.Estados.GetByIdAsync(id);
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");

            return _mapper.Map<EstadoViewModel>(estado);
        }

        public async Task<IEnumerable<EstadoViewModel>> GetAll(string? idTabla, string? descripcion)
        {
            Expression<Func<Estado, bool>> filter = e =>
                (string.IsNullOrEmpty(idTabla) || e.IdTabla == idTabla) &&
                (string.IsNullOrEmpty(descripcion) || e.Descripcion == descripcion);

            var estadosFiltrados = await _unitOfWork.Estados.FindAllAsync(filter);

            return _mapper.Map<IEnumerable<EstadoViewModel>>(estadosFiltrados);
        }

        public async Task Register(EstadoDTO estadoDTO)
        {
            var estado = _mapper.Map<Estado>(estadoDTO);
            await _unitOfWork.Estados.AddAsync(estado);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, EstadoDTO estadoDTO)
        {
            var estado = await _unitOfWork.Estados.GetByIdAsync(id);
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");

            _mapper.Map(estadoDTO, estado);

            await _unitOfWork.Estados.Update(estado);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var estado = await _unitOfWork.Estados.GetByIdAsync(id);
            if (estado == null)
                throw new KeyNotFoundException("Estado no encontrado.");

            _unitOfWork.Estados.Delete(estado);
            _unitOfWork.SaveChanges();
        }
    }
}
