using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.Models;

namespace TP_CP_5_Semester.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public DataController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("Tour/{id:long}")]
    public async Task<Tour> GetTourById(long id)
    {
        return await _db.Tours.Where(tour => tour.Id == id).FirstAsync();
    }

    [HttpGet("Fill")]
    public async Task<IActionResult> FillDbWithData()
    {
        for (var i = 0; i < 20; i++)
        {
            _db.Tours.Add(new Faker<Tour>("ru")
                .RuleFor(t => t.Name, f => f.Address.Country())
                .RuleFor(t => t.Price, f => f.Random.Int(0, 45000))
                .RuleFor(t => t.StartDate, f => f.Date.FutureDateOnly())
                .RuleFor(t => t.EndDate, (f, t) => f.Date.FutureDateOnly(refDate: t.StartDate)));
        }

        await _db.SaveChangesAsync();

        var tours = await _db.Tours.ToListAsync();
        var statuses = await _db.BookingStatuses.ToListAsync();
        var user = await _db.Users.FirstAsync();

        for (var i = 0; i < 100; i++)
        {
            _db.Bookings.Add(new Faker<Booking>("ru")
                .RuleFor(t => t.Amount, f => f.Random.Int(1, 20))
                .RuleFor(t => t.Tour, f => f.PickRandom(tours))
                .RuleFor(t => t.Status, f => f.PickRandom(statuses))
                .RuleFor(t => t.User, user));
        }
        
        await _db.SaveChangesAsync();

        return RedirectPermanent("/");
    }
}