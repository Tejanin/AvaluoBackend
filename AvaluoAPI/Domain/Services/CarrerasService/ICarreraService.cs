using AvaluoAPI.Presentation.DTOs.CarreraDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.CarreraService
{
    public interface ICarreraService
    {
        Task<PaginatedResult<CarreraViewModel>> GetAll(string? nombreCarrera, int? idEstado, int? idArea, int? idCoordinadorCarrera, int? page, int? recordsPerPage);
        Task<CarreraViewModel> GetById(int id);
        Task Register(CarreraDTO CarreraDTO);
        Task Update(int id, CarreraModifyDTO CarreraDTO);
        Task Delete(int id);
        Task UpdatePEOs(int id, string nuevosPEOs);
        Task<IEnumerable<AsignaturaCarreraViewModel>> GetSubjectByCareer(int? idCarrera);
        Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetMapaCompetencias(int idCarrera, int idTipoCompetencia);
    }

}
