using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using NuGet.Protocol.Core.Types;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.TipoMetodoEvaluacionRepositories
{
    public class MetodoEvaluacionRepository : Repository<MetodoEvaluacion>, IMetodoEvaluacionRepository
    {
        public MetodoEvaluacionRepository(AvaluoDbContext dbContext) : base(dbContext)
        {

        }
    }
}
