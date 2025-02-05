using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.UsuariosService
{
    public interface IUsuarioService
    {
        Task<string> Login(LoginDTO user);
        void Register(UsuarioDTO userDTO);
        Task Update(UsuarioDTO user);
        void Desactivate(int id);
        Task<UsuarioViewModel> GetById(int id);
        Task<IEnumerable<UsuarioViewModel>> GetAll(int? estado, int? area, int? rol);
        Task<UsuarioViewModel> Find();

    }
}
