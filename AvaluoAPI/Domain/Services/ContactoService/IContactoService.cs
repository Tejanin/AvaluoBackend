using AvaluoAPI.Presentation.DTOs.ContactoDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.ContactoService
{
    public interface IContactoService
    {
        Task<IEnumerable<ContactoViewModel>> GetAllByUserId(int userId);

        Task<ContactoViewModel> GetById(int id);

        Task Register(ContactoDTO contactoDTO);

        Task Update(int id, ContactoDTO contactoDTO);

        Task Delete(int id);
    }
}
