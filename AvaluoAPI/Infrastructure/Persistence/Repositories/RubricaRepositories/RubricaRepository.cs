using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.RubricaRepositories
{
    public class RubricaRepository: Repository<Rubrica>,IRubricaRepository
    {
        private readonly DapperContext _dapperContext;
        public RubricaRepository(AvaluoDbContext context, DapperContext dapperContext): base(context)
        {
            _dapperContext = dapperContext;
        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        
    }
}
