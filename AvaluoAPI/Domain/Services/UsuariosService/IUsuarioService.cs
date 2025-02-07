using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;

namespace AvaluoAPI.Domain.Services.UsuariosService
{
    public interface IUsuarioService
    {
        Task<TokenConfig> Login(LoginDTO user);
        Task Register(UsuarioDTO userDTO);
        Task Update(UsuarioDTO user);
        Task Desactivate(int id);
        Task ChangePassword(int id, string newPassword);
        Task Activate(int id);
        Task<UsuarioViewModel> GetById(int id);
        Task<IEnumerable<UsuarioViewModel>> GetAll(int? estado, int? area, int? rol);
        Task<UsuarioViewModel> Find();

    }
}
