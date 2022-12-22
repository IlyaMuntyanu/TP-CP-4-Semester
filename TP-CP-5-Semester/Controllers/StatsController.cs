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
        if (User.Identity.IsAuthenticated && (await _db.Users.ToListAsync()).Count > 0)
            ViewBag.UserBalance =
                (await _db.Users.Where(user => user.Email == User.Identity.Name).FirstAsync()).Balance;

        ViewBag.Stats = await _db.Bookings.Include(b => b.Tour).GroupBy(b => b.Tour.Id).Select(b => new Booking
        {
            Tour = b.First().Tour,
            Amount = b.Sum(booking => booking.Amount)
        }).ToListAsync();
        
        return View();
    }
}