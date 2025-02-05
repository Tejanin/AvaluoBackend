using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;

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

        public void Desactivate(int id)
        {
            var user = _context.Set<Usuario>().Find(id);  
            user.IdEstado = 6;
            user.FechaEliminacion = DateTime.Now;
            _context.Set<Usuario>().Update(user);


        }

        public async Task<IEnumerable<UsuarioViewModel>> GetAllUsuarios(int? estado, int? area, int? rol)
        {
            using var connection = _dapperContext.CreateConnection();

            var query = @"
                            SELECT 
                                u.*, 
                                e.Descripcion AS EstadoDescripcion, 
                                a.Nombre AS AreaNombre, 
                                r.Descripcion AS RolDescripcion,
                                r.EsProfesor,
                                r.EsSupervisor,
                                r.EsCoordinadorArea,
                                r.EsCoordinadorCarrera,
                                r.EsAdmin,
                                r.EsAux,
                                r.VerInformes,
                                r.VerListaDeRubricas,
                                r.ConfigurarFechas,
                                r.VerManejoCurriculum
                            FROM Usuarios u
                            LEFT JOIN Estados e ON u.IdEstado = e.Id
                            LEFT JOIN Areas a ON u.IdArea = a.Id
                            LEFT JOIN Roles r ON u.IdRol = r.Id
                            WHERE (@IdEstado IS NULL OR u.IdEstado = @IdEstado)
                              AND (@IdArea IS NULL OR u.IdArea = @IdArea)
                              AND (@IdRol IS NULL OR u.IdRol = @IdRol)";

            var parametros = new { IdEstado = estado, IdArea = area, IdRol = rol };

            return await connection.QueryAsync<UsuarioViewModel>(query, parametros);
        }
    }
}
