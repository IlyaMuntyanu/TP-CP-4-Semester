using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.Models;

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
        return View();
    }

    public async Task<IActionResult> DeleteTour(long id)
    {
        await _db.Tours.Where(tour => tour.Id == id).ExecuteDeleteAsync();
        return RedirectPermanent("/");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}