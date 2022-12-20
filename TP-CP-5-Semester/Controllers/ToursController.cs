using Microsoft.AspNetCore.Mvc;

namespace TP_CP_5_Semester.Controllers;

public class ToursController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}