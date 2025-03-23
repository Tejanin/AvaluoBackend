using Avaluo.Infrastructure.Data.Models;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Presentation.DTOs.InformeDTOs;
using AvaluoAPI.Presentation.ViewModels;

namespace AvaluoAPI.Domain.Services.InformeService
{
    public interface IInformeService
    {
        Task<PaginatedResult<InformeViewModel>> GetAll(int? idTipo, int? idCarrera, string? nombre, int? año, char? trimestre, string? periodo, int? page, int? recordsPerPage);
        Task<InformeViewModel> GetById(int id);
        Task Register(InformeDTO InformeDTO);
        Task RegistrarInformeGenerado(InformeDesempeñoViewModel informe, string pdfPath);
    }
}
