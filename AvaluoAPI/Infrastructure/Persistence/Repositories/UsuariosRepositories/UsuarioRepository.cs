using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly DapperContext _dapperContext;
        public UsuarioRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext; 
        }
   
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public void Activate(int id)
        {
            var user = _context.Set<Usuario>().Find(id);
            user.IdEstado = 5;
            user.FechaEliminacion = DateTime.Now;
            _context.Set<Usuario>().Update(user);
        }

        public void Desactivate(int id)
        {
            var user = _context.Set<Usuario>().Find(id);  
            user.IdEstado = 6;
            user.FechaEliminacion = DateTime.Now;
            _context.Set<Usuario>().Update(user);


        }


        public async Task<Usuario> GetUsuarioWithRol(string username)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = @"
                            SELECT 
                                u.*,
                                r.*
                            FROM usuario u
                            LEFT JOIN roles r ON u.IdRol = r.Id
                            WHERE u.Username = @Username";

            var usuarios = await connection.QueryAsync<Usuario, Rol, Usuario>(
                query,
                (usuario, rol) =>
                {
                    usuario.Rol = rol;
                    return usuario;
                },
                new { Username = username },
                splitOn: "Id"
            );

            return usuarios.FirstOrDefault();
        }
        public async Task<UsuarioViewModel> GetUsuarioById(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = @"
                            SELECT 
                                u.Id,
                                u.Username,
                                u.Email,
                                u.Nombre,
                                u.Apellido,
                                u.CV,
                                u.Foto,
                                e.Descripcion AS Estado,
                                a.Descripcion AS Area,
                                COALESCE(r.Descripcion, 'No asignado') AS Rol,
                                COALESCE(c.Nombre, 'N/A') AS SO
                            FROM usuario u
                            LEFT JOIN estado e ON u.IdEstado = e.Id
                            LEFT JOIN areas a ON u.IdArea = a.Id
                            LEFT JOIN roles r ON u.IdRol = r.Id
                            LEFT JOIN competencia c ON u.IdSO = c.Id
                            WHERE u.Id = @Id";
            var parametros = new { Id = id };
            return await connection.QuerySingleOrDefaultAsync<UsuarioViewModel>(query, parametros);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Set<Usuario>().AnyAsync(u => u.Email == email);   
        }

        public Task<bool> Exists(int id)
        {
            return _context.Set<Usuario>().AnyAsync(u => u.Id == id);   
        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAllUsuarios(int? estado, int? area, int? rol)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = @"
                            SELECT 
                                u.Id,
                                u.Username,
                                u.Email,
                                u.Nombre,
                                u.Apellido,
                                u.CV,
                                u.Foto,
                                e.Descripcion AS Estado,
                                a.Descripcion AS Area,
                                COALESCE(r.Descripcion, 'No asignado') AS Rol,
                                COALESCE(c.Nombre, 'N/A') AS SO,
                                con.Id,
                                con.NumeroContacto
                            FROM usuario u
                            LEFT JOIN estado e ON u.IdEstado = e.Id
                            LEFT JOIN areas a ON u.IdArea = a.Id
                            LEFT JOIN roles r ON u.IdRol = r.Id
                            LEFT JOIN competencia c ON u.IdSO = c.Id
                            LEFT JOIN contacto con ON u.Id = con.Id_Usuario
                            WHERE (@IdEstado IS NULL OR u.IdEstado = @IdEstado)
                                AND (@IdArea IS NULL OR u.IdArea = @IdArea)
                                AND (@IdRol IS NULL OR u.IdRol = @IdRol)";

            var usuariosDictionary = new Dictionary<int, UsuarioViewModel>();

            await connection.QueryAsync<UsuarioViewModel, ContactoViewModel, UsuarioViewModel>(
                query,
                (usuario, contacto) =>
                {
                    if (!usuariosDictionary.TryGetValue(usuario.Id, out var usuarioEntry))
                    {
                        usuarioEntry = usuario;
                        usuarioEntry.Contactos = new List<ContactoViewModel>();
                        usuariosDictionary.Add(usuario.Id, usuarioEntry);
                    }

                    if (contacto != null)
                    {
                        usuarioEntry.Contactos.Add(contacto);
                    }

                    return usuarioEntry;
                },
                new { IdEstado = estado, IdArea = area, IdRol = rol },
                splitOn: "Id"
            );

            return usuariosDictionary.Values;
        }

        public async Task<bool> EsProfesor(int id)
        {
            var usuario = await _context.Set<Usuario>()
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");

            if (usuario.IdRol == null || usuario.Rol == null)
                return false;

            return usuario.Rol.EsProfesor;
        }

        
    }
}
