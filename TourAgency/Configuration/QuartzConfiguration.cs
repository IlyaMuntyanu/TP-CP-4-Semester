using Quartz;
using TP_CP_5_Semester.ScheduledTasks;

namespace TP_CP_5_Semester.Configuration;

public static class QuartzConfiguration
{
    private static IScheduler? _scheduler;

    public static async Task Configure(string connectionString)
    {
        _scheduler = await SchedulerBuilder.Create()
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

        await _scheduler.Start();
        await RegisterJobs();
    }

    private static async Task RegisterJobs()
    {
        var markBookingsAsFinishedJob = JobBuilder
            .Create<MarkBookingsAsFinished>()
            .WithIdentity("markBookingAsFinished")
            .Build();

        var everydayTrigger = TriggerBuilder
            .Create()
            .WithIdentity("everyday")
            .StartNow()
            .WithSimpleSchedule(options =>
            {
                options.WithIntervalInHours(24);
                options.RepeatForever();
            })
            .Build();
        
        ArgumentNullException.ThrowIfNull(_scheduler);
        if (!await _scheduler.CheckExists(markBookingsAsFinishedJob.Key))
        {
            await _scheduler.ScheduleJob(markBookingsAsFinishedJob, everydayTrigger);
        }
    }
}