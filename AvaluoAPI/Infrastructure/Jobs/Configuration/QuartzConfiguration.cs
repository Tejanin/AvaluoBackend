using Quartz;

namespace AvaluoAPI.Infrastructure.Jobs.Configuration
{
    public static class QuartzConfiguration
    {
        public static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                // Job para activar las Rubricas segun la fecha de inicio

                //var jobKey = new JobKey("InsertRubricasJob");
                //q.AddJob<InsertRubricasJob>(opts => opts.WithIdentity(jobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(jobKey)
                //    .WithIdentity("InsertRubricasJob-trigger")
                //    .StartNow());


                // Job para desactivar Rubricas segun la fecha de cierre

                //var jobKey = new JobKey("DesactivateRubricasJob");
                //q.AddJob<InsertRubricasJob>(opts => opts.WithIdentity(jobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(jobKey)
                //    .WithIdentity("DesactivateRubricasJob-trigger")
                //    .StartNow());

                // Job para generar informes de desempeño de SO trimestral

                // Job para generar informe Self-Study-Report

             

            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
