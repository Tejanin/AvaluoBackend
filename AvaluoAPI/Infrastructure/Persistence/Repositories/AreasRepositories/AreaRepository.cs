using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AreasRepositories
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        private readonly DapperContext _dapperContext;
        public AreaRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public async Task<IEnumerable<AreaViewModel>> GetAllAreas()
        {
            using var connection = _dapperContext.CreateConnection();

            const string query = @"
        SELECT 
            a.Id,
            a.Descripcion,
            a.FechaCreacion,
            a.UltimaEdicion,
            u.Id,
            u.Username,
            u.Email,
            u.Nombre,
            u.Apellido,
            e.Descripcion as Estado,
            ar.Descripcion as Area,
            r.Descripcion as Rol,
            so.Descripcion as SO,
            u.CV,
            u.Foto,
            c.Id,
            c.NumeroContacto
        FROM areas a
        LEFT JOIN usuario u ON a.IdCoordinador = u.Id
        LEFT JOIN estado e ON u.IdEstado = e.Id
        LEFT JOIN areas ar ON u.IdArea = ar.Id
        LEFT JOIN rol r ON u.IdRol = r.Id
        LEFT JOIN so ON u.IdSO = so.Id
        LEFT JOIN contacto c ON u.Id = c.Id_Usuario
        ORDER BY a.Id";

            var areaDict = new Dictionary<int, AreaViewModel>();
            var userDict = new Dictionary<int, UsuarioViewModel>();

            await connection.QueryAsync<AreaViewModel, UsuarioViewModel, ContactoViewModel, AreaViewModel>(
                query,
                (area, usuario, contacto) =>
                {
                    if (!areaDict.TryGetValue(area.Id, out var areaEntry))
                    {
                        areaEntry = area;
                        areaDict.Add(area.Id, areaEntry);
                    }

                    if (usuario != null)
                    {
                        if (!userDict.TryGetValue(usuario.Id, out var userEntry))
                        {
                            userEntry = usuario;
                            userEntry.Contactos = new List<ContactoViewModel>();
                            userDict.Add(usuario.Id, userEntry);
                            areaEntry.Coordinador = userEntry;
                        }

                        if (contacto != null)
                        {
                            userEntry.Contactos.Add(contacto);
                        }
                    }

                    return areaEntry;
                },
                splitOn: "Id,Id");

            return areaDict.Values;
        }
    }
}
