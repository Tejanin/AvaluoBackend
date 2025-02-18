using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.SOEvaluacionRepositories
{
    public class SOEvaluacionRepository: Repository<SOEvaluacion>, ISOEvaluacionRepository
    {
        private readonly DapperContext _dapperContext;
        public SOEvaluacionRepository(AvaluoDbContext dbContext, DapperContext dapperContext) : base(dbContext)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        public Task<IEnumerable<SOEvaluacion>> GetSOEvaluaciones()
        {
            throw new NotImplementedException();
        }
    }
   
}
