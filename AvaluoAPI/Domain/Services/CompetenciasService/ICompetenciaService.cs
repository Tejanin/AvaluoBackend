﻿using AvaluoAPI.Presentation.DTOs.CompetenciaDTOs;
using AvaluoAPI.Presentation.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.CompetenciasService
{
    public interface ICompetenciaService
    {
        Task<CompetenciaViewModel> GetById(int id); // Obtiene una competencia por su ID.
        Task<IEnumerable<CompetenciaViewModel>> GetAll(string? nombre, string? acron, string? titulo, int? idTipo, int? idEstado); // Obtiene todas las competencias disponibles.
        Task Register(CompetenciaDTO competenciaDTO); // Registra una nueva competencia.
        Task Update(int id, CompetenciaModifyDTO competenciaDTO); // Actualiza los datos de una competencia existente.
        Task Delete(int id); // Elimina una competencia por su ID.
    }
}
