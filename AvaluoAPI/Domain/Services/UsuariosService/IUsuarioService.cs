using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities.JWT;

namespace AvaluoAPI.Domain.Services.UsuariosService
{
    public interface IUsuarioService
    {
        Task<TokenConfig> Login(LoginDTO user);
        Task Register(UsuarioDTO userDTO);
        Task Update(int id,ModifyUsuarioDTO user);
        Task Desactivate(int id);
        Task RequestPasswordChange();
        Task ChangePassword(string newPassword);
        Task UpdatePfp(int id, IFormFile file);
        Task UpdateCv(int id, IFormFile file);
        Task ChangePassword(int id, string newPassword);
        Task Activate(int id);
        Task<UsuarioViewModel> GetById(int id);
        Task<IEnumerable<UsuarioViewModel>> GetAll(int? estado, int? area, int? rol);
        Task<UsuarioViewModel> Find();

    }
}
