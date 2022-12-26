using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;

namespace TP_CP_5_Semester.Controllers;

public class ToursController : Controller
{
    private readonly ApplicationDbContext _db;

    public ToursController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated && (await _db.Users.ToListAsync()).Count <= 0) return View();

        ViewBag.BookingsList = await _db.Bookings.Where(b => b.User.Email == User.Identity.Name)
            .Include(b => b.Tour)
            .Include(b => b.Status)
            .OrderBy(b => b.Tour.Name)
            .ToListAsync();

        return View();
    }

    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var booking = await _db.Bookings.Where(b => b.Id == bookingId)
            .Include(b => b.Tour)
            .Include(b => b.Status)
            .Include(b => b.User)
            .FirstAsync();

        if (booking.Status.Name == "Оплачено")
        {

            var discount = 1.0;
            
            if ((booking.Tour.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
            {
                discount = 0.8;
            }
            
            // booking.User.Balance += (int)(booking.Amount * booking.Tour.Price * discount);
        }

        booking.Status = await _db.BookingStatuses.Where(bs => bs.Name == "Отменено").FirstAsync();
        booking.Tour.Leftover += booking.Amount;
        await _db.SaveChangesAsync();

        return RedirectPermanent("/Tours");
    }

    public async Task<IActionResult> PayForBooking(int bookingId)
    {
        var booking = await _db.Bookings.Where(b => b.Id == bookingId)
            .Include(b => b.Tour)
            .Include(b => b.User)
            .FirstAsync();

        var discount = 1.0;

        if ((booking.Tour.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
        {
            discount = 0.8;
        }

        var tourPrice = (int)(booking.Amount * booking.Tour.Price * discount);

        // if (booking.User.Balance < tourPrice) return RedirectPermanent("/Tours");
        // booking.User.Balance -= tourPrice;
        booking.Status = await _db.BookingStatuses.Where(bs => bs.Name == "Оплачено").FirstAsync();
        await _db.SaveChangesAsync();

        return RedirectPermanent("/Tours");
    }
}