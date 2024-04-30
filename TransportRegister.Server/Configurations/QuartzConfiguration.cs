using Microsoft.EntityFrameworkCore;
using Quartz;
using TransportRegister.Server.Data;

namespace TransportRegister.Server.Configurations
{
    public static class QuartzConfiguration
    {
        public static void AddQuartzConfiguration(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("UpdateBadPointsJob");

                q.AddJob<UpdateBadPointsJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("UpdateBadPointsJob-trigger")
                    // .WithCronSchedule("0 * * ? * *")); // every minute
                    .WithCronSchedule("0 0/1 * 1/1 * ? *")); // every hour

        });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }

    public class UpdateBadPointsJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateBadPointsJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var oneYearAgo = DateTime.Now.AddYears(-1);

            var drivers = await dbContext.Drivers
                .Where(d => d.LastCrimeCommited.HasValue && d.LastCrimeCommited.Value <= oneYearAgo
                         && (!d.LastPointsDeduction.HasValue || d.LastPointsDeduction.Value <= oneYearAgo))
                .ToListAsync();

            foreach (var driver in drivers)
            {
                if (driver.BadPoints > 0)
                {
                    driver.BadPoints = Math.Max(0, driver.BadPoints - 4);
                    driver.LastPointsDeduction = DateTime.Now;
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
