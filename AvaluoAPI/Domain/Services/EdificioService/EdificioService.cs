using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.EdificioDTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.EdificioService
{
    public class EdificioService : IEdificioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EdificioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Delete(int id)
        {
            var edificio = await _unitOfWork.Edificios.GetByIdAsync(id);
            if (edificio == null)
                throw new KeyNotFoundException("Edificio no encontrado.");

            _unitOfWork.Edificios.Delete(edificio);
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<EdificioViewModel>> GetAll()
        {
            var edificio = await _unitOfWork.Edificios.GetAllEdificios();
            return _mapper.Map<IEnumerable<EdificioViewModel>>(edificio);
        }

        public async Task<EdificioViewModel> GetById(int id)
        {
            var edificio = await _unitOfWork.Edificios.GetEdificioById(id);
            if (edificio == null)
                throw new KeyNotFoundException("Edificio no encontrado.");

            return _mapper.Map<EdificioViewModel>(edificio);
        }

        public async Task Register(EdificioDTO edificioDTO)
        {
            var edificio = _mapper.Map<Edificio>(edificioDTO);
            var edificioExiste = await _unitOfWork.Edificios.GetByIdAsync(edificio.Id);
            if (edificioExiste != null)
                throw new AggregateException("Edificio ya existe.");

            await _unitOfWork.Edificios.AddAsync(edificio);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, EdificioDTO edificioDTO)
        {
            var edificio = await _unitOfWork.Edificios.GetByIdAsync(id);
            if (edificio == null)
                throw new KeyNotFoundException("Edificio no encontrado.");

            edificio.Nombre = edificioDTO.Nombre;
            edificio.CantAulas = edificioDTO.CantAulas;
            edificio.Acron = edificioDTO.Acron;
            edificio.Ubicacion = edificioDTO.Ubicacion;
            edificio.IdArea = edificioDTO.IdArea;
            edificio.IdEstado = edificioDTO.IdEstado;
            edificio.UltimaEdicion = DateTime.Now;

            await _unitOfWork.Edificios.Update(edificio);
            _unitOfWork.SaveChanges();
        }
    }
}
