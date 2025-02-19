using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Persistence.Repositories.AreaRepositories;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AreasRepositories
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        public AreaRepository(AvaluoDbContext dbContext) : base(dbContext)
        {

        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
