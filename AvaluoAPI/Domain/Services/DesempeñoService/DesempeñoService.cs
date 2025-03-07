using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.ViewModels;
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
        public async Task<IEnumerable<InformeDesempeñoViewModel>> GenerarInformeDesempeño(int? año, string? periodo, int? idAsignatura, int? idSO = null)
        {
            return await _unitOfWork.Desempeños.GenerarInformeDesempeño(año, periodo, idAsignatura);
        }


    }
}
