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
        if (!User.Identity.IsAuthenticated) return View();

        ViewBag.BookingsList = await _db.Bookings.Where(b => b.User.Email == User.Identity.Name)
            .Include(b => b.Tour)
            .Include(b => b.Status)
            .OrderBy(b => b.Tour.Name)
            .ToListAsync();
        ViewBag.UserBalance = (await _db.Users.Where(u => u.Email == User.Identity.Name).FirstAsync()).Balance;

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
            booking.User.Balance += booking.Amount * booking.Tour.Price;
        }

        booking.Status = await _db.BookingStatuses.Where(bs => bs.Name == "Отменено").FirstAsync();
        await _db.SaveChangesAsync();

        return RedirectPermanent("/Tours");
    }

    public async Task<IActionResult> PayForBooking(int bookingId)
    {
        var booking = await _db.Bookings.Where(b => b.Id == bookingId)
            .Include(b => b.Tour)
            .Include(b => b.User)
            .FirstAsync();

        booking.User.Balance -= booking.Amount * booking.Tour.Price;

        booking.Status = await _db.BookingStatuses.Where(bs => bs.Name == "Оплачено").FirstAsync();
        await _db.SaveChangesAsync();

        return RedirectPermanent("/Tours");
    }
}