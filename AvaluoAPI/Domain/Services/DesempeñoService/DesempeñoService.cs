using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Presentation.ViewModels;
using AvaluoAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaluoAPI.Domain.Services.DesempeñoService
{
    public class DesempeñoService : IDesempeñoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PdfHelper _pdfHelper;

        public DesempeñoService(IUnitOfWork unitOfWork, PdfHelper pdfHelper)
        {
            _unitOfWork = unitOfWork;
            _pdfHelper = pdfHelper;
        }

        public async Task<IEnumerable<InformeDesempeñoViewModel>> GenerarInformeDesempeño(int? año, string? periodo, int? idAsignatura, int? idSO = null)
        {
            var informe = await _unitOfWork.Desempeños.GenerarInformeDesempeño(año, periodo, idAsignatura, idSO);

            if (informe == null || !informe.Any())
                throw new KeyNotFoundException("No hay datos disponibles para el informe solicitado.");

            return informe;
        }

        public async Task<string> GenerarYGuardarPdfInforme(
            IEnumerable<InformeDesempeñoViewModel> informe,
            int? año,
            string? periodo,
            int? idAsignatura,
            int? idSO)
        {
            // 1. Construir la ruta de almacenamiento
            string añoStr = año?.ToString() ?? "Desconocido";
            var rutaBuilder = new RutaInformeBuilder("Desempeño", añoStr);

            // 2. Construir el nombre del archivo
            string fileName = $"Informe_Desempeño_{añoStr}_{periodo ?? "Todos"}_{idAsignatura ?? 0}_{idSO ?? 0}";

            // 3. Generar y guardar el PDF
            return await _pdfHelper.GenerarYGuardarPdfAsync("Informes/InformeDesempeño", informe, rutaBuilder, fileName);
        }


    }
}