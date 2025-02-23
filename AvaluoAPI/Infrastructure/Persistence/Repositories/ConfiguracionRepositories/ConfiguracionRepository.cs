using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.ConfiguracionRepositories
{
    public class ConfiguracionRepository: Repository<ConfiguracionEvaluaciones>, IConfiguracionRepository
    {
        public ConfiguracionRepository(AvaluoDbContext context) : base(context)
        {
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
