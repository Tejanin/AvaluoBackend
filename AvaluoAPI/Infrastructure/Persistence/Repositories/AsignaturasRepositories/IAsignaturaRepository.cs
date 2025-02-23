using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;
using System.Threading.Tasks;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.AsignaturasRepositories
{
    public interface IAsignaturaRepository : IRepository<Asignatura>
    {
        Task<AsignaturaViewModel?> GetAsignaturaById(int id);  // Obtener una asignatura por ID con Estado y Área

        Task<PaginatedResult<AsignaturaViewModel>> GetAsignaturas( string? codigo, string? nombre, int? idEstado, int? idArea, int? page, int? recordsPerPage); // Obtener todas las asignaturas con filtros y paginación
    }
}
