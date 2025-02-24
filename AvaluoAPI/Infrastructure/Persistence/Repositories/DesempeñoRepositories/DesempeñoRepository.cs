using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.IDesempeñoRepositories
{
    public class DesempeñoRepository: Repository<Desempeno>, IDesempeñoRepository
    {
        private readonly DapperContext _dapperContext;
        public DesempeñoRepository(AvaluoDbContext context, DapperContext dapperContext): base(context)
        {
            _dapperContext= dapperContext;
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
