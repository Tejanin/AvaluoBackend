using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.PIService
{
    public class PiService : IPiService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PiService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PIViewModel> GetPI(int id)
        {
           return _mapper.Map<PIViewModel>(await _unitOfWork.PIs.FindAsync(p => p.Id == id));
        }

        public async Task<IEnumerable<PIViewModel>> GetPIsBySO(int so)
        {
            return _mapper.Map<IEnumerable<PIViewModel>>(await _unitOfWork.PIs.FindAllAsync(p => p.IdSO == so));
        }
    }
}
