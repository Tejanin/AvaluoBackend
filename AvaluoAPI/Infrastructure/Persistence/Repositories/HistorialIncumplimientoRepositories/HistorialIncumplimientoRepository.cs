using Avaluo.Infrastructure.Data;
using Avaluo.Infrastructure.Data.Models;
using Avaluo.Infrastructure.Persistence.Repositories.Base;
using AvaluoAPI.Domain.Helper;
using AvaluoAPI.Infrastructure.Data.Contexts;
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
    }
}
