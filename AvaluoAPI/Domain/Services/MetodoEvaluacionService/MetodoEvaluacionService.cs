using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.MetodoEvaluacionService
{
    public class MetodoEvaluacionService : IMetodoEvaluacionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MetodoEvaluacionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Delete(int id)
        {
            var metodoEvaluacion = await _unitOfWork.MetodoEvaluacion.GetByIdAsync(id);
            if (metodoEvaluacion == null)
                throw new KeyNotFoundException("Tipo de Informe no encontrado.");

            _unitOfWork.MetodoEvaluacion.Delete(metodoEvaluacion);
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<MetodoEvaluacion>> GetAll()
        {

            var metodoEvaluacions = await _unitOfWork.MetodoEvaluacion.GetAllAsync();
            return _mapper.Map<IEnumerable<MetodoEvaluacion>>(metodoEvaluacions);
        }

        public async Task<MetodoEvaluacion> GetById(int id)
        {
            var metodoEvaluacion = await _unitOfWork.MetodoEvaluacion.GetByIdAsync(id);
            if (metodoEvaluacion == null)
                throw new KeyNotFoundException("Metodo de Evaluacion no encontrado.");

            return _mapper.Map<MetodoEvaluacion>(metodoEvaluacion);
        }

        public async Task Register(MetodoEvaluacionDTO metodoEvaluacionDTO)
        {
            var metodoEvaluacion = _mapper.Map<MetodoEvaluacion>(metodoEvaluacionDTO);
            await _unitOfWork.MetodoEvaluacion.AddAsync(metodoEvaluacion);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, MetodoEvaluacionDTO metodoEvaluacionDTO)
        {
            var metodoEvaluacion = await _unitOfWork.MetodoEvaluacion.GetByIdAsync(id);
            if (metodoEvaluacion == null)
                throw new KeyNotFoundException("Metodo de Evaluacion no encontrado.");

            metodoEvaluacion.Descripcion = metodoEvaluacionDTO.Descripcion;
            await _unitOfWork.MetodoEvaluacion.Update(metodoEvaluacion);
            _unitOfWork.SaveChanges();
        }
    }
}
