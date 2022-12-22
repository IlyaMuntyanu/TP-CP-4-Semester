using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;

namespace TP_CP_5_Semester.Controllers;

public class StatsController : Controller
{
    private readonly ApplicationDbContext _db;
    // GET
    public StatsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {

        if (User.Identity.IsAuthenticated)
            ViewBag.UserBalance = (await _db.Users.Where(user => user.Email == User.Identity.Name).FirstAsync()).Balance;
        return View();
    }
}