using AvaluoAPI.Presentation.DTOs.CarreraDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.CarreraService
{
    public interface ICarreraService
    {
        Task<PaginatedResult<CarreraViewModel>> GetAll(string? nombreCarrera, int? idEstado, int? idArea, int? idCoordinadorCarrera, int? año, string? peos, int? page, int? recordsPerPage); // Obtiene una lista paginada de carreras con filtros opcionales.
        Task<CarreraViewModel> GetById(int id); // Obtiene una carrera por su ID.
        Task Register(CarreraDTO CarreraDTO); // Registra una nueva carrera.
        Task Update(int id, CarreraModifyDTO CarreraDTO); // Actualiza una carrera existente con base en su ID.
        Task Delete(int id);  // Elimina una carrera por su ID.
        Task UpdatePEOs(int id, string nuevosPEOs);  // Actualiza los PEOs (Program Educational Objectives) de una carrera específica.
        Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetMapaCompetencias(int idCarrera, int idTipoCompetencia); // Obtiene el mapa de competencias de una carrera.
    }

}
