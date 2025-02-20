using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories
{
    public class TipoInformeRepository: Repository<TipoInforme>, ITipoInformeRepository
    {
        public TipoInformeRepository(AvaluoDbContext dbContext) : base(dbContext)
        {
            
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
