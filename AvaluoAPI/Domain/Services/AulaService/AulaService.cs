using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.AulaDTOs;
using AvaluoAPI.Presentation.DTOs.MetodoEvaluacionDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;

namespace AvaluoAPI.Domain.Services.AulaService
{
    public class AulaService : IAulaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AulaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task Delete(int id)
        {
            var aula = await _unitOfWork.Aulas.GetByIdAsync(id);
            if (aula == null)
                throw new KeyNotFoundException("Aula no encontrada.");

            _unitOfWork.Aulas.Delete(aula);
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<AulaViewModel>> GetAll(string? descripcion, int? idEdificio, int? idEstado)
        {

            var aulas = await _unitOfWork.Aulas.GetAulas(descripcion, idEdificio, idEstado);
            return _mapper.Map<IEnumerable<AulaViewModel>>(aulas);
        }

        public async Task<AulaViewModel> GetById(int id)
        {
            var aula = await _unitOfWork.Aulas.GetAulasById(id);
            if (aula == null)
                throw new KeyNotFoundException("Aula no encontrada.");

            return _mapper.Map<AulaViewModel>(aula);
        }

        public async Task Register(AulaDTO aulaDTO)
        {
            var aula = _mapper.Map<Aula>(aulaDTO);
            var aulaExist = await _unitOfWork.Aulas.GetAllAsync();

            foreach (var aulaIn in aulaExist)
            {
                if (aulaIn.Descripcion == aulaDTO.Descripcion)
                    throw new ArgumentException("Ya existe un Aula con esa descripcion.");
            }

            await _unitOfWork.Aulas.AddAsync(aula);
            _unitOfWork.SaveChanges();
        }

        public async Task Update(int id, AulaDTO aulaDTO)
        {
            var aula = await _unitOfWork.Aulas.GetByIdAsync(id);
            var aulaExist = await _unitOfWork.Aulas.GetAllAsync();
            if (aula == null)
                throw new KeyNotFoundException("Aula no encontrada.");
            foreach (var aulaIn in aulaExist)
            {
                if (aulaIn.Descripcion == aulaDTO.Descripcion)
                    throw new ArgumentException("Ya existe un Metodo de Evaluacion con esa descripcion (ES).");
            }

            aula.Descripcion = aulaDTO.Descripcion;
            await _unitOfWork.Aulas.Update(aula);
            _unitOfWork.SaveChanges();
        }
    }
}
