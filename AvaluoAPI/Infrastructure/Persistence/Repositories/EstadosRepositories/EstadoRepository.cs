using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories
{
    public class EstadoRepository : Repository<Estado>, IEstadoRepository
    {
        public EstadoRepository(AvaluoDbContext dbContext) : base(dbContext)
        {
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
