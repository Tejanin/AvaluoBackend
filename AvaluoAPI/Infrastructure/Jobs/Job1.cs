using Avaluo.Infrastructure.Persistence.UnitOfWork;
using Quartz;

namespace AvaluoAPI.Infrastructure.Jobs
{
    public class Job1 : BaseJob
    {
        //private readonly IServicio1 _servicio1;

        public Job1(ILogger<Job1> logger, IUnitOfWork unitOfWork )
            : base(logger, unitOfWork)
        {
            //_servicio1 = servicio1;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            try
            {
               //await _servicio1.RealizarTarea();
                _logger.LogInformation("Job1 ejecutado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Job1");
                throw;
            }
        }
    }
}
