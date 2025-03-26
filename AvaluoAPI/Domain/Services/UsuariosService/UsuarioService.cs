using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Application.Handlers;
using AvaluoAPI.Presentation.DTOs.CarreraDTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using AvaluoAPI.Utilities.JWT;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static System.Net.WebRequestMethods;

namespace AvaluoAPI.Domain.Services.UsuariosService
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly FileHandler _fileHandler;
        private readonly IEmailService _emailService;
        private TokenConfig _tokens;
        public UsuarioService(IUnitOfWork unitOfWork,IMapper mapper, IJwtService jwtService, FileHandler fileHandler,  IEmailService emailService)
        {
            _emailService = emailService;
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

        public async Task<PaginatedResult<UsuarioViewModel>> GetAll(int? estado, int? area, int? rol, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Usuarios.GetAllUsuarios(estado, area, rol, page, recordsPerPage);
        }

        public async Task<UsuarioViewModel> GetById(int id)
        {
            var user = await _unitOfWork.Usuarios.GetUsuarioById(id);
            if (user == null) throw new KeyNotFoundException("El usuario no existe");

            // Verificar si el usuario tiene una foto y convertir la ruta a una URL accesible
            if (!string.IsNullOrEmpty(user.Foto))
            {
                // Modificar la ruta del archivo para que sea una URL de API
                // Extraer solo el nombre del archivo de la ruta completa
                string fileName = Path.GetFileName(user.Foto);

                // Construir la URL relativa para la API
                user.Foto = $"AvaluoFiles/Usuarios/{user.Id}/{fileName}";
            }

            return user;

        }
        public async Task RegisterRange(List<UsuarioDTO> usuarios)
        {
            var users = new List<Usuario>();

            foreach (var u in usuarios)
            {
                await Register(u);
            }
        }
        public async Task ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
           
            var user = await _unitOfWork.Usuarios.GetByIdAsync(changePasswordDTO.Id);
            if (user == null) throw new KeyNotFoundException("El usuario no existe");
            user.Salt = DateTime.Now.ToString();
            user.HashedPassword = Hasher.Hash(changePasswordDTO.NewPassword, user.Salt);
            _unitOfWork.SaveChanges();

        }

        public async Task ChangePassword( string newPassword)
        {
            string id = _jwtService.GetClaimValue("Id")!;
            var user = await _unitOfWork.Usuarios.GetByIdAsync(int.Parse(id));
            if (user == null) throw new KeyNotFoundException("El usuario no existe");
            user.Salt = DateTime.Now.ToString();
            user.HashedPassword = Hasher.Hash(newPassword, user.Salt);
            await _unitOfWork.Usuarios.Update(user);
            _unitOfWork.SaveChanges();
        }

        public async Task<TokenConfig> Login(LoginDTO user)
        {
            var userDB = await _unitOfWork.Usuarios.GetUsuarioWithRol(user.Username); 
            if (userDB == null) throw new KeyNotFoundException("El usuario no existe");
            if (Hasher.Verify(user.Contraseña, userDB.Salt, userDB.HashedPassword) != true) throw new ValidationException("Contraseña incorrecta");

            _tokens = _jwtService.GenerateAuthTokens(userDB, userDB.Rol);

            return _tokens;



        }

        public async Task Register(UsuarioDTO userDTO)
        {   
            userDTO.Contraseña = Hasher.Hash(userDTO.Contraseña, userDTO.Salt);
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
            if (usuarioDTO.Area.HasValue && usuarioDTO.Area != 0 && usuario.IdArea != usuarioDTO.Area)
                usuario.IdArea = usuarioDTO.Area;

            if (usuarioDTO.Rol.HasValue && usuarioDTO.Rol != 0 && usuario.IdRol != usuarioDTO.Rol)
                usuario.IdRol = usuarioDTO.Rol;


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
            RutaUsuarioBuilder rutaBuilder = new RutaUsuarioBuilder($"{usuario.Id}");

            (bool exito, string mensaje, string ruta, _) = await _fileHandler.Upload(
                file,
                new List<string> { ".jpg", ".png" },
                rutaBuilder,
                nombre => $"PFP_{usuario.Id}"
            );

            // Necesito la ruta para guardarla

            if (!exito) throw new Exception(mensaje);

            usuario.Foto = ruta;
            await _unitOfWork.Usuarios.Update(usuario);
            _unitOfWork.SaveChanges();
        }

        public async Task UpdateCv(int id, IFormFile file)
        {
            var usuarioTask = await _unitOfWork.Usuarios.GetUsuarioWithRolById(id);
            if (await _unitOfWork.Usuarios.Exists(id) != true) throw new KeyNotFoundException("El usuario no existe");

            if (usuarioTask?.Rol?.EsProfesor != true && usuarioTask?.Rol?.EsSupervisor != true)
                throw new KeyNotFoundException("El usuario especificado no tiene el rol de profesor para subir CV");

            var usuario = usuarioTask;

            // Guardar el archivo
            RutaUsuarioBuilder rutaBuilder = new RutaUsuarioBuilder($"{usuario.Id}");

            (bool exito, string mensaje, string ruta, _) = await _fileHandler.Upload(
                file,
                new List<string> { ".pdf" },
                rutaBuilder,
                nombre => $"CV_{usuario.Id}"
            );

            // Necesito la ruta para guardarla

            if (!exito) throw new Exception(mensaje);

            usuario.CV = ruta;
            await _unitOfWork.Usuarios.Update(usuario);
            _unitOfWork.SaveChanges();
        }

        public async Task RequestPasswordChange(string email)
        {
            var user = await _unitOfWork.Usuarios.FindAsync(u => u.Email == email);

            if (user == null) throw new ArgumentNullException("El email enviado no corresponde a ningun usuario");
            var token = _jwtService.GenerateEmailToken(user);
            string url = $"http://localhost:3000/auth/email-recover-password/{token.JwtToken}";
            await _emailService.SendEmailAsync(email,"Cuenta Avalúo - Solicitud de cambio de contraseña",url,true);
        }
    }
}
