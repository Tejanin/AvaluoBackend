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
            var metodoEvaluacionExist = await _unitOfWork.MetodoEvaluacion.GetAllAsync();
            foreach (var metodos in metodoEvaluacionExist)
            {
                if (metodos.DescripcionES == metodoEvaluacionDTO.DescripcionES)
                    throw new ArgumentException("Ya existe un Metodo de Evaluacion con esa descripcion (ES).");
            }

            foreach (var metodos in metodoEvaluacionExist)
            {
                if (metodos.DescripcionEN == metodoEvaluacionDTO.DescripcionEN)
                    throw new ArgumentException("Ya existe un Metodo de Evaluacion con esa descripcion (EN).");
            }

            await _unitOfWork.MetodoEvaluacion.AddAsync(metodoEvaluacion);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, MetodoEvaluacionDTO metodoEvaluacionDTO)
        {
            var metodoEvaluacion = await _unitOfWork.MetodoEvaluacion.GetByIdAsync(id);
            var metodoEvaluacionExist = await _unitOfWork.MetodoEvaluacion.GetAllAsync();
            if (metodoEvaluacion == null)
                throw new KeyNotFoundException("Metodo de Evaluacion no encontrado.");
            foreach (var metodos in metodoEvaluacionExist)
            {
                if (metodos.DescripcionES == metodoEvaluacionDTO.DescripcionES)
                    throw new ArgumentException("Ya existe un Metodo de Evaluacion con esa descripcion (ES).");
            }

            foreach (var metodos in metodoEvaluacionExist)
            {
                if (metodos.DescripcionEN == metodoEvaluacionDTO.DescripcionEN)
                    throw new ArgumentException("Ya existe un Metodo de Evaluacion con esa descripcion (EN).");
            }

            metodoEvaluacion.DescripcionES = metodoEvaluacionDTO.DescripcionES;
            await _unitOfWork.MetodoEvaluacion.Update(metodoEvaluacion);
            _unitOfWork.SaveChanges();
        }
    }
}
