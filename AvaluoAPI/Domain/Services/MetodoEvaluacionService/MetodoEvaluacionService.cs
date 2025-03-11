using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using AvaluoAPI.Presentation.ViewModels;
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

        public async Task<IEnumerable<MetodoEvaluacionViewModel>> GetAll()
        {

            var metodoEvaluacions = await _unitOfWork.MetodoEvaluacion.GetAllAsync();
            return _mapper.Map<IEnumerable<MetodoEvaluacionViewModel>>(metodoEvaluacions);
        }

        public async Task<MetodoEvaluacionViewModel> GetById(int id)
        {
            var metodoEvaluacion = await _unitOfWork.MetodoEvaluacion.GetByIdAsync(id);
            if (metodoEvaluacion == null)
                throw new KeyNotFoundException("Metodo de Evaluacion no encontrado.");

            return _mapper.Map<MetodoEvaluacionViewModel>(metodoEvaluacion);
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
        public async Task<IEnumerable<MetodoEvaluacionViewModel>> GetMetodosEvaluacionPorSO(int idSO)
        {
            var so = await _unitOfWork.Competencias.GetByIdAsync(idSO);
            if (so == null)
                throw new KeyNotFoundException("Student Outcome (SO) no encontrado.");

            var metodosEvaluacion = await _unitOfWork.SOEvaluaciones.GetMetodosEvaluacionPorSO(idSO);
            if (metodosEvaluacion == null || !metodosEvaluacion.Any())
                throw new KeyNotFoundException("No se encontraron métodos de evaluación para el Student Outcome especificado.");

            return _mapper.Map<IEnumerable<MetodoEvaluacionViewModel>>(metodosEvaluacion);
        }

        public async Task RegisterSOEvaluacion(int idSO, int idMetodoEvaluacion)
        {
            var so = await _unitOfWork.Competencias.GetByIdAsync(idSO);
            if (so == null)
                throw new KeyNotFoundException($"El Student Outcome con ID {idSO} no existe.");

            var metodoEvaluacion = await _unitOfWork.MetodoEvaluacion.GetByIdAsync(idMetodoEvaluacion);
            if (metodoEvaluacion == null)
                throw new KeyNotFoundException($"El Método de Evaluación con ID {idMetodoEvaluacion} no existe.");

            var relacionesExistentes = await _unitOfWork.SOEvaluaciones.GetAllAsync();
            bool existeRelacion = relacionesExistentes.Any(r => r.IdSO == idSO && r.IdMetodoEvaluacion == idMetodoEvaluacion);
            if (existeRelacion)
                throw new ArgumentException("Ya existe una relación entre el SO y el Método de Evaluación especificados.");

            var soEvaluacion = new SOEvaluacion { IdSO = idSO, IdMetodoEvaluacion = idMetodoEvaluacion };

            await _unitOfWork.SOEvaluaciones.AddAsync(soEvaluacion);
            _unitOfWork.SaveChanges();
        }

        public async Task DeleteSOEvaluacion(int idSO, int idMetodoEvaluacion)
        {
            var relacionesExistentes = await _unitOfWork.SOEvaluaciones.GetAllAsync();
            var soEvaluacion = relacionesExistentes.FirstOrDefault(r => r.IdSO == idSO && r.IdMetodoEvaluacion == idMetodoEvaluacion);
            if (soEvaluacion == null)
                throw new KeyNotFoundException("No se encontró una relación SO - Método de Evaluación para eliminar.");

            _unitOfWork.SOEvaluaciones.Delete(soEvaluacion);
            _unitOfWork.SaveChanges();
        }


    }
}
