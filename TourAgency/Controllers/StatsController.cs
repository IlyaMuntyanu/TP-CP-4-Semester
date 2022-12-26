using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.Models;

namespace TP_CP_5_Semester.Controllers;

public class StatsController : Controller
{
    private readonly ApplicationDbContext _db;

    public StatsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var paidStatus = await _db.BookingStatuses.Where(bs => bs.Name == "Оплачено").FirstAsync();

        ViewBag.Stats = await _db.Bookings
            .Include(b => b.Tour)
            .Where(b => b.Status == paidStatus)
            .GroupBy(b => b.Tour.Id)
            .Select(b => new Booking
        {
            Tour = b.First().Tour,
            Amount = b.Sum(booking => booking.Amount)
        }).ToListAsync();
        
        return View();
    }
}