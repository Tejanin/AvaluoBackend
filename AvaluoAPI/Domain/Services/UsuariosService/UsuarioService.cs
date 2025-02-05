using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.UsuariosService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UsuarioService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public void Desactivate(int id)
        {
           _unitOfWork.Usuarios.Desactivate(id);
        }

        public Task<UsuarioViewModel> Find()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAll(int? estado, int? area, int? rol)
        {
            throw new NotImplementedException();
        }

        public Task<UsuarioViewModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(LoginDTO user)
        {
            throw new NotImplementedException();
        }

        public void Register(UsuarioDTO userDTO)
        {   
                userDTO.Password = Hasher.Hash(userDTO.Password, userDTO.Salt);
                var user = _mapper.Map<Usuario>(userDTO);
                _unitOfWork.Usuarios.Add(user);
                _unitOfWork.SaveChanges();
        }

        public Task Update(UsuarioDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
