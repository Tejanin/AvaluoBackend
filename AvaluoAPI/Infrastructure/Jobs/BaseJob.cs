using Avaluo.Infrastructure.Persistence.UnitOfWork;
using Quartz;

namespace AvaluoAPI.Infrastructure.Jobs
{
    public abstract class BaseJob : IJob
    {
        protected readonly ILogger<BaseJob> _logger;
        protected readonly IUnitOfWork _unitOfWork;

        protected BaseJob(ILogger<BaseJob> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public abstract Task Execute(IJobExecutionContext context);

        protected async Task ReprogramarJob(IJobExecutionContext context, DateTime nuevaFecha)
        {
            try
            {
                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"{context.JobDetail.Key.Name}-trigger")
                    .StartAt(nuevaFecha)
                    .Build();

                await context.Scheduler.RescheduleJob(context.Trigger.Key, trigger);
                _logger.LogInformation("Job reprogramado para {fecha}", nuevaFecha);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reprogramar el job");
                throw;
            }
        }
    }
}
