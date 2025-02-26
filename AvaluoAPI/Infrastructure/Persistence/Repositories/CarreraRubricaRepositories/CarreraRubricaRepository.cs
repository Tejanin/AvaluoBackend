using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CarreraRubricaRepositories
{
    public class CarreraRubricaRepository: Repository<CarreraRubrica>, ICarreraRubricaRepository
    {
        public CarreraRubricaRepository(AvaluoDbContext context) : base(context)
        {
        }

        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
