using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.AreaDTOs;
using AvaluoAPI.Presentation.DTOs.AulaDTOs;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.AreaService
{
    public class AreaService: IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AreaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AreaViewModel> GetById(int id)
        {
            var area = await _unitOfWork.Areas.GetAreaById(id);
            if (area == null)
                throw new KeyNotFoundException("Area no encontrada.");

            return area;
        }

        public async Task<PaginatedResult<AreaViewModel>> GetAll(string? descripcion, int? idCoordinador, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Areas.GetAreas(descripcion, idCoordinador, page, recordsPerPage);
        }

        public async Task Register(AreaDTO areaDTO)
        {
            if (areaDTO.Descripcion == "" || areaDTO.Descripcion == null)
                throw new KeyNotFoundException("Por favor ingrese una descripcion.");

            var area = _mapper.Map<Area>(areaDTO);

            await _unitOfWork.Areas.AddAsync(area);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, AreaDTO areaDTO)
        {
            var area = await _unitOfWork.Areas.GetByIdAsync(id);
            if (area == null)
                throw new KeyNotFoundException("Area no encontrada.");

            if (areaDTO.Descripcion == "" || areaDTO.Descripcion == null)
                areaDTO.Descripcion = area.Descripcion;

            area.Descripcion = areaDTO.Descripcion;
            area.IdCoordinador = areaDTO.IdCoordinador;
            area.UltimaEdicion = DateTime.UtcNow;

            await _unitOfWork.Areas.Update(area);
            _unitOfWork.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var area = await _unitOfWork.Areas.GetByIdAsync(id);
            if (area == null)
                throw new KeyNotFoundException("Area no encontrada.");

            _unitOfWork.Areas.Delete(area);
            _unitOfWork.SaveChanges();
        }
    }
}
