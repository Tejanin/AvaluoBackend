using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AvaluoAPI.Domain.Services.UsuariosService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;   
        public UsuarioService(IUnitOfWork unitOfWork,IMapper mapper, IJwtService jwtService, AvaluoDbContext context)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Activate(int id)
        {
            if (await _unitOfWork.Usuarios.Exists(id) != true) throw new KeyNotFoundException("El usuario no existe");

            try
            {
                _unitOfWork.Usuarios.Activate(id);
                _unitOfWork.SaveChanges();
            }
            catch
            {
                throw new Exception("Error al activar el usuario");
            }
        }

        public async Task Desactivate(int id)
        {
            if(await _unitOfWork.Usuarios.Exists(id) != true) throw new KeyNotFoundException("El usuario no existe");

            try
            {
                _unitOfWork.Usuarios.Desactivate(id);
                _unitOfWork.SaveChanges();
            }
            catch 
            {
                throw new Exception("Error al desactivar el usuario");
            }

        }

        public Task<UsuarioViewModel> Find()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAll(int? estado, int? area, int? rol)
        {
            return await _unitOfWork.Usuarios.GetAllUsuarios(estado, area, rol);
        }

        public async Task<UsuarioViewModel> GetById(int id)
        {
            var user = await _unitOfWork.Usuarios.GetUsuarioById(id);
            if (user == null) throw new KeyNotFoundException("El usuario no existe");
            return user;

        }

        public async Task ChangePassword(int id, string newPassword)
        {
            var user = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("El usuario no existe");
            user.HashedPassword = Hasher.Hash(newPassword, user.Salt);
            _unitOfWork.SaveChanges();
        }

        public async Task<TokenConfig> Login(LoginDTO user)
        {
            var userDB = await _unitOfWork.Usuarios.GetUsuarioWithRol(user.Username); 
            if (userDB == null) throw new KeyNotFoundException("El usuario no existe");
            if (Hasher.Verify(user.Password, userDB.Salt, userDB.HashedPassword) != true) throw new ValidationException("Contraseña incorrecta");

            

            return _jwtService.GenerateTokens(userDB,userDB.Rol);



        }

        public async Task Register(UsuarioDTO userDTO)
        {   
            userDTO.Password = Hasher.Hash(userDTO.Password, userDTO.Salt);
            var user = _mapper.Map<Usuario>(userDTO);
            if (await _unitOfWork.Usuarios.EmailExists(userDTO.Email))
            {
               throw new ValidationException("Existe otro usuario con ese email");
            }

            _unitOfWork.Usuarios.Add(user);
            _unitOfWork.SaveChanges();
        }

        public Task Update(UsuarioDTO user)
        {
            throw new NotImplementedException();
        }
    }
}
