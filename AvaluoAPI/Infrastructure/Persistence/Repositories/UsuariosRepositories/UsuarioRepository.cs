using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.UsuariosRepositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AvaluoDbContext dbContext) : base(dbContext)
        {

        }
    
    }
}
