using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ContactoRepositories
{
    public interface IContactoRepository : IRepository<Contacto>
    {
        Task<IEnumerable<ContactoViewModel>> GetContactosByUserId(int userId);

        Task<ContactoViewModel?> GetContactoById(int id);
    }
}
