using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Persistence.Repositories.EstadosRepositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Estado> GetEstadoByTablaName(string idtabla, string nombre)
        {
           return await _context.Set<Estado>().FirstOrDefaultAsync(e => e.IdTabla == idtabla && e.Descripcion == nombre);
        }
    }
}
