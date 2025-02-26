﻿using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.CarrerasRepositories
{
    public interface ICarreraRepository : IRepository<Carrera>
    {
        Task<CarreraViewModel?> GetCarreraById(int id);
        Task<PaginatedResult<CarreraViewModel>> GetCarreras(string? nombreCarrera, int? idEstado, int? idArea, int? idCoordinadorCarrera, int? año, string? peos, int? page, int? recordsPerPage);
        Task<IEnumerable<AsignaturaConCompetenciasViewModel>> GetMapaCompetencias(int idCarrera, int idTipoCompetencia);
    }
}
