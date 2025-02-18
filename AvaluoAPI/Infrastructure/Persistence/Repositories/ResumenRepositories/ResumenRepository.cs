using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ResumenRepositories
{
    public class ResumenRepository: Repository<Resumen>, IResumenRepository
    {
        private readonly DapperContext _dapperContext;
        public ResumenRepository(AvaluoDbContext context, DapperContext dapperContext): base(context)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
