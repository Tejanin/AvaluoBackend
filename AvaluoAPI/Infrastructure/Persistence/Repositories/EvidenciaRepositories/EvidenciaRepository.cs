using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.EvidenciaRepositories
{
    public class EvidenciaRepository: Repository<Evidencia>, IEvidenciaRepository
    {
        public EvidenciaRepository(AvaluoDbContext context) : base(context)
        {

        }
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
    }
}
