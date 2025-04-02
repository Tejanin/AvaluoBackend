using AvaluoAPI.Presentation.DTOs.AsignaturaCarreraDTOs;
using AvaluoAPI.Presentation.DTOs.AsignaturaDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.AsignaturaService
{
    public interface IAsignaturaService
    {
        Task<PaginatedResult<AsignaturaViewModel>> GetAll(string? codigo, string? nombre, int? idEstado, int? idArea, int? page, int? recordsPerPage); // Obtiene una lista paginada de asignaturas con filtros opcionales.
        Task<AsignaturaViewModel> GetById(int id); // Obtiene una asignatura por su ID.
        Task<PaginatedResult<AsignaturaViewModel>> GetSubjectByCareer(int idCarrera, int? page, int? recordsPerPage); // Obtiene una lista paginada de asignaturas asociadas a una carrera específica.
        Task RegisterSubjectByCareer(AsignaturaCarreraDTO asignaturaCarreraDTO);
        Task DeleteGetSubjectByCareer(AsignaturaCarreraDTO asignaturaCarreraDTO);
        Task UpdateDocument(int id, IFormFile file, string tipoDocumento);
        Task Register(AsignaturaDTO asignaturaDTO); // Registra una nueva asignatura.
        Task Update(int id, AsignaturaModifyDTO asignaturaDTO); // Actualiza una asignatura existente con base en su ID.
        Task Delete(int id); // Elimina una asignatura por su ID.
    }

}
