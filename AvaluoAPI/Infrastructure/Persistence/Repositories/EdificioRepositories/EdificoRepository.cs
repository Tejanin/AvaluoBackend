using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.EdificioRepositories
{
    public class EdificoRepository: Repository<Edificio>, IEdificioRespository
    {
        public EdificoRepository(AvaluoDbContext dbContext) : base(dbContext)
        {

        }
    }
}
