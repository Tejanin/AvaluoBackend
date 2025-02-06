using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<IEnumerable<UsuarioViewModel>> GetAllUsuarios(int? estado, int? area, int? rol);
        void Desactivate(int id);

        Task<bool> EmailExists(string email);
    }
}
