using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.RubricaViewModels;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.HistorialIncumplimientoService
{
    public class HistorialIncumplimientoService : IHistorialIncumplimientoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public HistorialIncumplimientoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<HistorialIncumplimientoViewModel>> GetAll(
            int? idUsuario,
            string? descripcion,
            DateTime? desde,
            DateTime? hasta,
            int? page,
            int? recordsPerPage)
        {
            return await _unitOfWork.HistorialIncumplimientos.GetHistorialIncumplimientos(
                idUsuario, descripcion, desde, hasta, page, recordsPerPage);
        }


    }
}
