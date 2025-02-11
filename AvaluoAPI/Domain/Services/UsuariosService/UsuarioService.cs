﻿using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using AvaluoAPI.Utilities.JWT;
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
        private readonly FileHandler _fileHandler;
        public UsuarioService(IUnitOfWork unitOfWork,IMapper mapper, IJwtService jwtService, FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
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

        public async Task Update(int id,ModifyUsuarioDTO usuarioDTO)
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id) ??
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");

            // Actualizamos solo si hay cambios, manteniendo los valores originales si no hay modificación
            if (!string.IsNullOrWhiteSpace(usuarioDTO.Username) && usuario.Username != usuarioDTO.Username)
                usuario.Username = usuarioDTO.Username;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Email) && usuario.Email != usuarioDTO.Email)
                usuario.Email = usuarioDTO.Email;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Nombre) && usuario.Nombre != usuarioDTO.Nombre)
                usuario.Nombre = usuarioDTO.Nombre;

            if (!string.IsNullOrWhiteSpace(usuarioDTO.Apellido) && usuario.Apellido != usuarioDTO.Apellido)
                usuario.Apellido = usuarioDTO.Apellido;

            // Para las relaciones, verificamos si el valor es diferente de null y diferente del valor actual
            if (usuarioDTO.IdArea.HasValue && usuarioDTO.IdArea != 0 && usuario.IdArea != usuarioDTO.IdArea)
                usuario.IdArea = usuarioDTO.IdArea;

            if (usuarioDTO.IdRol.HasValue && usuarioDTO.IdRol != 0 && usuario.IdRol != usuarioDTO.IdRol)
                usuario.IdRol = usuarioDTO.IdRol;


            // Actualizamos la fecha de última edición
            usuario.UltimaEdicion = DateTime.Now;

            await _unitOfWork.Usuarios.Update(usuario);
            _unitOfWork.SaveChanges();
        }

        public async Task UpdatePfp(int id, IFormFile file)
        {
            if(await _unitOfWork.Usuarios.Exists(id) != true) throw new KeyNotFoundException("El usuario no existe");

            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);

            // Guardar el archivo
            RutaUsuarioBuilder rutaBuilder = new RutaUsuarioBuilder($"{usuario.Username}_{usuario.Id}");

            (bool exito, string mensaje, string ruta) = await _fileHandler.Upload(
                file,
                new List<string> { ".jpg", ".png" },
                rutaBuilder,
                nombre => $"PFP_{usuario.Id}"
            );

            // Necesito la ruta para guardarla

            if (!exito) throw new Exception(mensaje);

            usuario.Foto = ruta;
            _unitOfWork.Usuarios.Update(usuario);
            _unitOfWork.SaveChanges();
        }

        public async Task UpdateCv(int id, IFormFile file)
        {
            var usuarioTask = _unitOfWork.Usuarios.GetByIdAsync(id);
            if (await _unitOfWork.Usuarios.Exists(id) != true) throw new KeyNotFoundException("El usuario no existe");

            if(await _unitOfWork.Usuarios.EsProfesor(id) != true) throw new Exception("No es profesor");

            var usuario = await usuarioTask;

            // Guardar el archivo
            RutaUsuarioBuilder rutaBuilder = new RutaUsuarioBuilder($"{usuario.Username}_{usuario.Id}");

            (bool exito, string mensaje, string ruta) = await _fileHandler.Upload(
                file,
                new List<string> { ".pdf" },
                rutaBuilder,
                nombre => $"CV_{usuario.Id}"
            );

            // Necesito la ruta para guardarla

            if (!exito) throw new Exception(mensaje);

            usuario.CV = ruta;
            _unitOfWork.Usuarios.Update(usuario);
            _unitOfWork.SaveChanges();
        }
    }
}
