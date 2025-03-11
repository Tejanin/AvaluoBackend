using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.InventarioDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Domain.Services.InventarioService
{
    public class InventarioService : IInventarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InventarioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<InventarioViewModel>> GetAll(string? descripcion, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Inventario.GetInventario(descripcion, page, recordsPerPage);
        }

        public async Task<InventarioViewModel> GetById(int id)
        {
            var inventario = await _unitOfWork.Inventario.GetInventarioById(id);
            if (inventario == null)
                throw new KeyNotFoundException("Inventario no encontrado.");

            return inventario;
        }

        public async Task Register(InventarioDTO inventarioDTO)
        {
            var inventario = _mapper.Map<Inventario>(inventarioDTO);
            inventario.FechaCreacion = DateTime.UtcNow;
            await _unitOfWork.Inventario.AddAsync(inventario);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, InventarioDTO inventarioDTO)
        {
            var inventario = await _unitOfWork.Inventario.GetByIdAsync(id);
            if (inventario == null)
                throw new KeyNotFoundException("Inventario no encontrado.");

            _mapper.Map(inventarioDTO, inventario);
            inventario.UltimaEdicion = DateTime.UtcNow;

            await _unitOfWork.Inventario.Update(inventario);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var inventario = await _unitOfWork.Inventario.GetByIdAsync(id);
            if (inventario == null)
                throw new KeyNotFoundException("Inventario no encontrado.");

            _unitOfWork.Inventario.Delete(inventario);
            _unitOfWork.SaveChanges();
        }

        public async Task<List<InventarioViewModel>> GetByLocation(int? edificioId, int? aulaId)
        {
            return await _unitOfWork.Inventario.GetInventarioByLocation(edificioId, aulaId);
        }

    }
}
