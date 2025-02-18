using Quartz;

namespace AvaluoAPI.Infrastructure.Jobs.Configuration
{
    public static class QuartzConfiguration
    {
        public static void ConfigureQuartz(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {

                //var jobKey = new JobKey("InsertRubricasJob");
                //q.AddJob<InsertRubricasJob>(opts => opts.WithIdentity(jobKey));
                //q.AddTrigger(opts => opts
                //    .ForJob(jobKey)
                //    .WithIdentity("InsertRubricasJob-trigger")
                //    .StartNow());
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}
