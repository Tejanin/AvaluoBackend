using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.InformesRepositories
{
    public class InformeRepository : Repository<Informe>, IInformeRepository
    {
        private readonly DapperContext _dapperContext;

        public InformeRepository(AvaluoDbContext context, DapperContext dapperContext) : base(context)
        {
            _dapperContext = dapperContext;
        }

        private AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }

        // Obtiene informes con filtros, incluye relaciones y devuelve un paginado con ViewModel.
        public async Task<PaginatedResult<InformeViewModel>> GetAllInformesAsync(
            int? idTipo,
            int? idCarrera,
            string? nombre,
            int? año,
            char? trimestre,
            string? periodo,
            int? page,
            int? recordsPerPage)
        {
            Expression<Func<Informe, bool>> filter = e =>
                (!idTipo.HasValue || e.IdTipo == idTipo.Value) &&
                (!idCarrera.HasValue || e.IdCarrera == idCarrera.Value) &&
                (!año.HasValue || e.Año == año.Value) &&
                (string.IsNullOrEmpty(nombre) || e.Nombre.Contains(nombre)) &&
                (!trimestre.HasValue || e.Trimestre == trimestre.Value) &&
                (string.IsNullOrEmpty(periodo) || e.Periodo.Contains(periodo));

            IQueryable<Informe> query = FindAllQuery(filter)
                .Include(i => i.TipoInforme)
                .Include(i => i.Carrera);

            var paginatedResult = await PaginateWithQuery(query, page, recordsPerPage);

            var result = paginatedResult.Convert(i => new InformeViewModel
            {
                Id = i.Id,
                Ruta = i.Ruta,
                FechaCreacion = i.FechaCreacion,
                Nombre = i.Nombre,
                Tipo = i.TipoInforme.Descripcion,
                Carrera = i.Carrera.NombreCarrera,
                Año = i.Año,
                Trimestre = i.Trimestre,
                Periodo = i.Periodo
            });

            return result;
        }
        public async Task<InformeViewModel?> GetInformeByIdAsync(int id)
        {
            var informe = await AvaluoDbContext.Informes
                .Include(i => i.TipoInforme)
                .Include(i => i.Carrera)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (informe == null)
                return null;

            return new InformeViewModel
            {
                Id = informe.Id,
                Ruta = informe.Ruta,
                FechaCreacion = informe.FechaCreacion,
                Nombre = informe.Nombre,
                Tipo = informe.TipoInforme.Descripcion,
                Carrera = informe.Carrera.NombreCarrera,
                Año = informe.Año,
                Trimestre = informe.Trimestre,
                Periodo = informe.Periodo
            };
        }

    }
}
