using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Data.Contexts;
using AvaluoAPI.Presentation.ViewModels;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Diagnostics.Metrics;

namespace AvaluoAPI.Infrastructure.Persistence.Repositories.HistorialIncumplimientoRepositories
{
    public class HistorialIncumplimientoRepository: Repository<HistorialIncumplimiento>, IHistorialIncumplimientoRepository
    {
        private readonly DapperContext _dapperContext;
        public AvaluoDbContext? AvaluoDbContext
        {
            get { return _context as AvaluoDbContext; }
        }
        public HistorialIncumplimientoRepository(AvaluoDbContext context, DapperContext dapperContext) : base(context)
        {
            _dapperContext = dapperContext;
        }

        public async Task InsertIncumplimientos(IEnumerable<Rubrica> rubricas)
        {
            var incumplimientos = new List<HistorialIncumplimiento>();

            foreach (var rubrica in rubricas)
            {
                var incumplimiento = new HistorialIncumplimiento
                {
                   Descripcion = $"El docente {rubrica.Profesor.Nombre} {rubrica.Profesor.Apellido} no completó el avaluo No.{rubrica.Id} del Student Outcome {rubrica.IdSO} de la asignatura {rubrica.Asignatura.Codigo}-{rubrica.Asignatura.Nombre} en el periodo {PeriodoExtensions.GetNombreTrimestre(rubrica.Periodo)} del {rubrica.Año}.",
                   IdUsuario = rubrica.IdProfesor,
                };
                incumplimientos.Add(incumplimiento);
            }

            await _context.Set<HistorialIncumplimiento>().AddRangeAsync(incumplimientos);
        }

        public async Task<PaginatedResult<HistorialIncumplimientoViewModel>> GetHistorialIncumplimientos(
            int? idUsuario,
            string? descripcion,
            DateTime? desde,
            DateTime? hasta,
            int? page,
            int? recordsPerPage)
        {
            using var connection = _dapperContext.CreateConnection();

            // Normalizar fechas para ignorar la hora
            var desdeInicio = desde?.Date;
            var hastaFinal = hasta?.Date.AddDays(1); 

            // Conteo total
            var countQuery = @"
            SELECT COUNT(*)
            FROM dbo.historial_incumplimiento h
            INNER JOIN dbo.usuario u ON h.Id_Usuario = u.Id
            WHERE (@IdUsuario IS NULL OR h.Id_Usuario = @IdUsuario)
              AND (@Descripcion IS NULL OR h.Descripcion LIKE '%' + @Descripcion + '%')
              AND (@Desde IS NULL OR h.Fecha >= @Desde)
              AND (@Hasta IS NULL OR h.Fecha < @Hasta)";

            int totalRecords = await connection.ExecuteScalarAsync<int>(countQuery, new
            {
                IdUsuario = idUsuario,
                Descripcion = descripcion,
                Desde = desdeInicio,
                Hasta = hastaFinal
            });

            if (totalRecords == 0)
            {
                return new PaginatedResult<HistorialIncumplimientoViewModel>(Enumerable.Empty<HistorialIncumplimientoViewModel>(), 1, 0, 0);
            }

            // Cálculo de paginación
            int currentRecordsPerPage = recordsPerPage.HasValue && recordsPerPage > 0 ? recordsPerPage.Value : totalRecords;
            int currentPage = page.HasValue && page > 0 ? page.Value : 1;
            int offset = (currentPage - 1) * currentRecordsPerPage;

            // Query principal
            var query = @"
            SELECT 
                h.Id,
                h.Descripcion,
                h.Fecha,
                u.Nombre + ' ' + u.Apellido AS Usuario
            FROM dbo.historial_incumplimiento h
            INNER JOIN dbo.usuario u ON h.Id_Usuario = u.Id
            WHERE (@IdUsuario IS NULL OR h.Id_Usuario = @IdUsuario)
              AND (@Descripcion IS NULL OR h.Descripcion LIKE '%' + @Descripcion + '%')
              AND (@Desde IS NULL OR h.Fecha >= @Desde)
              AND (@Hasta IS NULL OR h.Fecha < @Hasta)
            ORDER BY h.Fecha DESC
            OFFSET @Offset ROWS FETCH NEXT @RecordsPerPage ROWS ONLY";

            var result = await connection.QueryAsync<HistorialIncumplimientoViewModel>(query, new
            {
                IdUsuario = idUsuario,
                Descripcion = descripcion,
                Desde = desdeInicio,
                Hasta = hastaFinal,
                Offset = offset,
                RecordsPerPage = currentRecordsPerPage
            });

            return new PaginatedResult<HistorialIncumplimientoViewModel>(result, currentPage, currentRecordsPerPage, totalRecords);
        }

    }
}
