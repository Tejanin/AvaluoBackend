using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.ContactoDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Domain.Services.ContactoService
{
    public class ContactoService : IContactoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContactoViewModel>> GetAllByUserId(int userId)
        {
            var contactos = await _unitOfWork.Contactos.GetContactosByUserId(userId);
            return _mapper.Map<IEnumerable<ContactoViewModel>>(contactos);
        }

        public async Task<ContactoViewModel> GetById(int id)
        {
            var contacto = await _unitOfWork.Contactos.GetByIdAsync(id);
            if (contacto == null)
                throw new KeyNotFoundException("Contacto no encontrado.");

            return _mapper.Map<ContactoViewModel>(contacto);
        }

        public async Task Register(ContactoDTO contactoDTO)
        {
            var contacto = _mapper.Map<Contacto>(contactoDTO);
            await _unitOfWork.Contactos.AddAsync(contacto);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, ContactoDTO contactoDTO)
        {
            var contacto = await _unitOfWork.Contactos.GetByIdAsync(id);
            if (contacto == null)
                throw new KeyNotFoundException("Contacto no encontrado.");

            _mapper.Map(contactoDTO, contacto);
            await _unitOfWork.Contactos.Update(contacto);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var contacto = await _unitOfWork.Contactos.GetByIdAsync(id);
            if (contacto == null)
                throw new KeyNotFoundException("Contacto no encontrado.");

            _unitOfWork.Contactos.Delete(contacto);
            _unitOfWork.SaveChanges();
        }
    }
}
