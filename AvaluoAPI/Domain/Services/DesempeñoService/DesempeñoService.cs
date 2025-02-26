using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Domain.Services.DesempeñoService
{
    public class DesempeñoService: IDesempeñoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DesempeñoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
    }
}
