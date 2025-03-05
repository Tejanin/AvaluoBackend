using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Presentation.ViewModels.MapaCompetenciasViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.CompetenciasService
{
    public interface ICompetenciaService
    {
        Task<CompetenciaViewModel> GetById(int id); // Obtiene una competencia por su ID.
        Task<PaginatedResult<CompetenciaViewModel>> GetAll(string? nombre, string? acron, string? titulo, int? idTipo, int? idEstado, int? page, int? recordsPerPage); // Obtiene todas las competencias filtradas y paginadas
        Task Register(CompetenciaDTO competenciaDTO); // Registra una nueva competencia.
        Task Update(int id, CompetenciaModifyDTO competenciaDTO); // Actualiza los datos de una competencia existente.
        Task Delete(int id); // Elimina una competencia por su ID.
        Task<IEnumerable<MapaCompetenciaViewModel>> GetMapaCompetencias(int idCarrera, int idTipoCompetencia); // Obtiene el mapa de competencias de una carrera.
        Task<bool> UpdateEstadoMapaCompetencia(int idAsignatura, int idCompetencia, UpdateEstadoMapaCompetenciaDTO dto); // Actualiza el estado de una competencia en el mapa de competencias de una asignatura.

    }
}
