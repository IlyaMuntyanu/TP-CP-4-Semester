using Microsoft.EntityFrameworkCore;
using Quartz;
using TP_CP_5_Semester.Data;

namespace TP_CP_5_Semester.ScheduledTasks;

public class MarkBookingsAsFinished : IJob
{
    private readonly ApplicationDbContext _db;

    public MarkBookingsAsFinished(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var finishedBookings = _db.Bookings
            .Include(b => b.Status)
            .Where(b => b.Status.Name == "Забронировано" &&
                        (b.Tour.EndDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days == 0);


        var finishedStatus = await _db.BookingStatuses.Where(bs => bs.Name == "Завершено").FirstAsync();

        foreach (var booking in finishedBookings)
        {
            booking.Status = finishedStatus;
        }

        await _db.SaveChangesAsync();
    }
}
