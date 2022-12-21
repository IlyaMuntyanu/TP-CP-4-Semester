using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.Models;
using TP_CP_5_Semester.RequestBodies;

namespace TP_CP_5_Semester.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.ToursList = await _db.Tours.ToListAsync();
        if (User.Identity.IsAuthenticated)
            ViewBag.UserBalance = (await _db.Users.Where(user => user.Email == User.Identity.Name).FirstAsync()).Balance;
        return View();
    }

    public async Task<IActionResult> DeleteTour(long id)
    {
        await _db.Tours.Where(tour => tour.Id == id).ExecuteDeleteAsync();
        return RedirectPermanent("/");
    }

    public async Task<IActionResult> CreateTour(Tour tour)
    {
        await _db.Tours.AddAsync(tour);
        await _db.SaveChangesAsync();
        return RedirectPermanent("/");
    }

    public async Task<IActionResult> EditTour(Tour tour)
    {
        _db.Tours.Update(tour);
        await _db.SaveChangesAsync();
        return RedirectPermanent("/");
    }

    public async Task<IActionResult> OrderTour(OrderRequest body)
    {
        var user = await _db.Users.Where(user => user.UserName == body.Email).FirstAsync();
        var bookingStatus = await _db.BookingStatuses.Where(bs => bs.Name.Equals("Забронировано")).FirstAsync();
        var tour = await _db.Tours.FindAsync(body.TourId);
        ArgumentNullException.ThrowIfNull(tour);

        var booking = new Booking
        {
            Amount = body.Amount,
            Status = bookingStatus,
            Tour = tour,
            User = user
        };

        await _db.Bookings.AddAsync(booking);
        await _db.SaveChangesAsync();
        
        return RedirectPermanent("/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}