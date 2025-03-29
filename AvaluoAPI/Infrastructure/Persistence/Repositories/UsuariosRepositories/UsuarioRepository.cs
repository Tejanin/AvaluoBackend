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

        public async Task<Usuario> GetUsuarioWithRolById(int usuarioId)
        {
            using var connection = _dapperContext.CreateConnection();
            var query = @"
                    SELECT 
                        u.*,
                        r.*
                    FROM usuario u
                    LEFT JOIN roles r ON u.IdRol = r.Id
                    WHERE u.Id = @UsuarioId";
            var usuarios = await connection.QueryAsync<Usuario, Rol, Usuario>(
                query,
                (usuario, rol) =>
                {
                    usuario.Rol = rol;
                    return usuario;
                },
                new { UsuarioId = usuarioId },
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
            COALESCE(c.Nombre, 'N/A') AS SO,
            c2.Id,
            c2.NumeroContacto AS NumeroContacto
        FROM usuario u
        LEFT JOIN estado e ON u.IdEstado = e.Id
        LEFT JOIN areas a ON u.IdArea = a.Id
        LEFT JOIN roles r ON u.IdRol = r.Id
        LEFT JOIN competencia c ON u.IdSO = c.Id
        LEFT JOIN contacto c2 ON u.Id = c2.Id_Usuario
        WHERE u.Id = @Id";

            var parametros = new { Id = id };

            var usuarioDict = new Dictionary<int, UsuarioViewModel>();

            var resultado = await connection.QueryAsync<UsuarioViewModel, ContactoViewModel, UsuarioViewModel>(
                query,
                (usuario, contacto) =>
                {
                    if (!usuarioDict.TryGetValue(usuario.Id, out var usuarioEntry))
                    {
                        usuarioEntry = usuario;
                        usuarioEntry.Contactos = new List<ContactoViewModel>();
                        usuarioDict.Add(usuario.Id, usuarioEntry);
                    }

                    if (contacto != null && !string.IsNullOrEmpty(contacto.NumeroContacto))
                    {
                        usuarioEntry.Contactos.Add(contacto);
                    }

                    return usuarioEntry;
                },
                parametros,
                splitOn: "Id"
            );

            return usuarioDict.Values.FirstOrDefault();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.Set<Usuario>().AnyAsync(u => u.Email == email);   
        }

        public Task<bool> Exists(int id)
        {
            return _context.Set<Usuario>().AnyAsync(u => u.Id == id);   
        }

        public async Task<PaginatedResult<UsuarioViewModel>> GetAllUsuarios(int? estado, int? area, int? rol, int? page, int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            var countQuery = @"
                    SELECT COUNT(*)
                    FROM usuario u
                    LEFT JOIN estado e ON u.IdEstado = e.Id
                    LEFT JOIN areas a ON u.IdArea = a.Id
                    LEFT JOIN roles r ON u.IdRol = r.Id
                    LEFT JOIN competencia c ON u.IdSO = c.Id
                    LEFT JOIN contacto con ON u.Id = con.Id_Usuario
                    WHERE (@IdEstado IS NULL OR u.IdEstado = @IdEstado)
                      AND (@IdArea IS NULL OR u.IdArea = @IdArea)
                      AND (@IdRol IS NULL OR u.IdRol = @IdRol)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                IdEstado = estado,
                IdArea = area,
                IdRol = rol
            });

            if (totalRecords == 0)
            {
                return new PaginatedResult<UsuarioViewModel>(Enumerable.Empty<UsuarioViewModel>(), 1, 0, 0);
            }

            int currentPage = page.HasValue && page > 0 ? page.Value : 1;
            int recordsPerPageValue = recordsPerPage.HasValue && recordsPerPage.Value > 0 ? recordsPerPage.Value : totalRecords;
            int offset = (currentPage - 1) * recordsPerPageValue;

            var query = $@"
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
                                AND (@IdRol IS NULL OR u.IdRol = @IdRol)
                            ORDER BY u.Id
                            OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

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
                new { IdEstado = estado, IdArea = area, IdRol = rol, Offset = offset, RecordsPerPage = recordsPerPageValue },
                splitOn: "Id"
            );

            return new PaginatedResult<UsuarioViewModel>(usuariosDictionary.Values, currentPage, recordsPerPageValue, totalRecords);
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

        public async Task<bool> EsSupervisor(int id)
        {
            var usuario = await _context.Set<Usuario>()
                 .Include(u => u.Rol)
                 .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new KeyNotFoundException($"No se encontró el usuario con ID {id}");

            if (usuario.IdRol == null || usuario.Rol == null)
                return false;

            return usuario.Rol.EsSupervisor;
        }


    }
}
