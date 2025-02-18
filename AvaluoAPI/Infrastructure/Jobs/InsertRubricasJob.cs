using Avaluo.Infrastructure.Persistence.UnitOfWork;
using AvaluoAPI.Domain.Services.RubricasService;
using Quartz;

namespace AvaluoAPI.Infrastructure.Jobs
{
    public class InsertRubricasJob : BaseJob
    {
        private readonly ILogger<InsertRubricasJob> _logger;
        private readonly IRubricaService _rubricaService;

        public InsertRubricasJob(ILogger<InsertRubricasJob> logger,IUnitOfWork unitOfWork ,IRubricaService rubricaService): base(logger, unitOfWork)
        {
            _logger = logger;
            _rubricaService = rubricaService;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var proximaEjecucion = new DateTime(2025, 2, 18, 16, 05, 0);
                if (proximaEjecucion > DateTime.Now)
                {
                    await ReprogramarJob(context, proximaEjecucion);
                    _logger.LogInformation("Job reprogramado para {fecha}", proximaEjecucion);
                    return; // Importante: salir del método para evitar la ejecución inmediata
                }
                await _rubricaService.InsertRubricas();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar el job de prueba",DateTime.Now);

                await Task.CompletedTask;
            }
        }

        
    }
}
