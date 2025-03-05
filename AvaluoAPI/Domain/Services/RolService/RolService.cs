using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.RolDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Domain.Services.RolService
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RolService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<RolViewModel>> GetAll(string? descripcion, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Roles.GetRoles(descripcion, page, recordsPerPage);
        }

        public async Task<RolViewModel> GetById(int id)
        {
            var rol = await _unitOfWork.Roles.GetRolById(id);
            if (rol == null)
                throw new KeyNotFoundException("Rol no encontrado.");

            return rol;
        }

        public async Task Register(RolDTO rolDTO)
        {
            var rol = _mapper.Map<Rol>(rolDTO);
            await _unitOfWork.Roles.AddAsync(rol);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, RolDTO rolDTO)
        {
            var rol = await _unitOfWork.Roles.GetByIdAsync(id);
            if (rol == null)
                throw new KeyNotFoundException("Rol no encontrado.");

            _mapper.Map(rolDTO, rol);
            await _unitOfWork.Roles.Update(rol);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var rol = await _unitOfWork.Roles.GetByIdAsync(id);
            if (rol == null)
                throw new KeyNotFoundException("Rol no encontrado.");

            _unitOfWork.Roles.Delete(rol);
            _unitOfWork.SaveChanges();
        }
    }
}
