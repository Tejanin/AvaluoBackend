using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;


namespace AvaluoAPI.Infrastructure.Persistence.Repositories.TipoInformeRepositories
{
    public class TipoCompetenciaRepository: Repository<TipoCompetencia>, ITipoCompetenciaRepository
    {
        public TipoCompetenciaRepository(AvaluoDbContext dbContext) : base(dbContext)
        {   
        }
    }
}
