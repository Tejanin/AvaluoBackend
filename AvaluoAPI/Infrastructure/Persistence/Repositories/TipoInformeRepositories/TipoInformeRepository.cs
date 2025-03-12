using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;


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

        public async Task<TipoInforme?> GetTipoInformeByDescripcionAsync(string descripcion)
        {
            return await _context.Set<TipoInforme>()
                .FirstOrDefaultAsync(t => t.Descripcion.ToLower().Contains(descripcion.ToLower()));
        }
    }
}
