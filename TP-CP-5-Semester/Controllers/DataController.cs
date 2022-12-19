using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.Models;

namespace TP_CP_5_Semester.Controllers;

[ApiController]
[Route("[controller]")]
public class DataController: ControllerBase
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
}