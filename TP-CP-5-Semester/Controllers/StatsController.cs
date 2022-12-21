using Microsoft.AspNetCore.Mvc;

namespace TP_CP_5_Semester.Controllers;

public class StatsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}