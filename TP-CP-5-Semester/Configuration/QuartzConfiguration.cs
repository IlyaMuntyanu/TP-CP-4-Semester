using Quartz;

namespace TP_CP_5_Semester.Configuration;

public static class QuartzConfiguration
{
    public static async Task Configure(string connectionString)
    {
        var scheduler = await SchedulerBuilder.Create()
            .UseDefaultThreadPool(options => options.MaxConcurrency = 5)
            .UsePersistentStore(
                options =>
                {
                    options.UseProperties = true;
                    options.UsePostgres(connectionString);
                    options.UseClustering();
                    options.UseJsonSerializer();
                })
            .BuildScheduler();

        await scheduler.Start();
    }
}