using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Presentation.DTOs;
using AvaluoAPI.Presentation.DTOs.UserDTOs;
using AvaluoAPI.Presentation.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.TipoInformeService
{
    public interface ITipoInformeService
    {
        Task<TipoInformeViewModel> GetById(int id); // Obtiene un tipo de informe por su ID
        Task<PaginatedResult<TipoInformeViewModel>> GetAll(int? page, int? recordsPerPage);// Obtiene todos los tipos de informes paginados

        Task Register(TipoInformeDTO tipoInformeDTO); // Registra un nuevo tipo de informe

        Task Update(int id, TipoInformeDTO tipoInformeDTO); // Actualiza un tipo de informe existente

        Task Delete(int id); // Elimina un tipo de informe por su ID
    }
}
