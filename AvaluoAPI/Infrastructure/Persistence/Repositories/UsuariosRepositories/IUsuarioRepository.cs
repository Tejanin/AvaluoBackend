using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<PaginatedResult<UsuarioViewModel>> GetAllUsuarios(int? estado, int? area, int? rol, int? page, int? recordsPerPage);
        void Desactivate(int id);
        
        Task<bool> EsProfesor(int id);
        Task<Usuario> GetUsuarioWithRol(string username);
        Task<Usuario> GetUsuarioWithRolById(int usuarioId);
        Task<UsuarioViewModel> GetUsuarioById(int id);
        
        void Activate(int id);
        Task<bool> Exists(int id);
        Task<bool> EmailExists(string email);
    }
}
