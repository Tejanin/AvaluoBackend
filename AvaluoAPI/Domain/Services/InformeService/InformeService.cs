using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.DTOs.InformeDTOs;
using AvaluoAPI.Presentation.ViewModels;
using MapsterMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using AvaluoAPI.Domain.Helper;
using Microsoft.EntityFrameworkCore;

namespace AvaluoAPI.Domain.Services.InformeService
{
    public class InformeService : IInformeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InformeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<InformeViewModel>> GetAll(int? idTipo, int? idCarrera, string? nombre, int? año, char? trimestre, string? periodo, int? page, int? recordsPerPage)
        {
            return await _unitOfWork.Informes.GetAllInformesAsync(idTipo, idCarrera, nombre, año, trimestre, periodo, page, recordsPerPage);
        }

        public async Task<InformeViewModel> GetById(int id)
        {
            var Informe = await _unitOfWork.Informes.GetInformeByIdAsync(id);
            if (Informe == null)
                throw new KeyNotFoundException("Informe no encontrado.");

            return _mapper.Map<InformeViewModel>(Informe);
        }

        public async Task Register(InformeDTO InformeDTO)
        {
            var Informe = _mapper.Map<Informe>(InformeDTO);
            Informe.FechaCreacion = DateTime.UtcNow;
            await _unitOfWork.Informes.AddAsync(Informe);
            _unitOfWork.SaveChanges();
        }

        public async Task RegistrarInformeGenerado(InformeDesempeñoViewModel informe, string pdfPath)
        {
            var tipoDesempeño = await _unitOfWork.TiposInformes
                .GetTipoInformeByDescripcionAsync("desempeño");

            if (tipoDesempeño == null)
                throw new Exception("No se encontró un tipo de informe con la descripción 'Desempeño'.");

            int idTipo = tipoDesempeño.Id;

            var idCarreras = await _unitOfWork.AsignaturasCarreras
                .GetCarrerasIdsByAsignaturaId(informe.IdAsignatura);

            if (!idCarreras.Any())
                throw new Exception($"No se encontraron carreras para la asignatura {informe.IdAsignatura}");

            foreach (var idCarrera in idCarreras)
            {
                var informeDTO = new InformeDTO
                {
                    Nombre = Path.GetFileName(pdfPath),
                    Ruta = pdfPath,
                    IdTipo = idTipo,
                    IdCarrera = idCarrera,
                    Año = informe.Año,
                    Trimestre = Convert.ToChar(informe.Trimestre),
                    Periodo = "trimestral"
                };

                await Register(informeDTO);
            }
        }
    }
}
