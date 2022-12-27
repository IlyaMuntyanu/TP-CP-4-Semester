using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Configuration;
using TP_CP_5_Semester.Data;
using TP_CP_5_Semester.PaymentApiClient;
using TP_CP_5_Semester.PaymentApiClient.RequestBodies;

namespace TP_CP_5_Semester.Controllers;

public class ToursController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly Client _client;
    private readonly TourAgencyConfiguration _configuration;

    public ToursController(ApplicationDbContext db, Client client, TourAgencyConfiguration configuration)
    {
        _db = db;
        _client = client;
        _configuration = configuration;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity.IsAuthenticated && (await _db.Users.ToListAsync()).Count <= 0) return View();

        ViewBag.BookingsList = await _db.Bookings.Where(b => b.User.Email == User.Identity.Name)
            .Include(b => b.Tour)
            .Include(b => b.Status)
            .OrderBy(b => b.Tour.Name)
            .ToListAsync();

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

            var discount = 1.0;
            
            if ((booking.Tour.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
            {
                discount = 0.8;
            }
            
            await _client.TransferCard(new TransferBody
            {
                FromCardNumber = _configuration.PaymentConfiguration.CardNumber,
                FromCvc = _configuration.PaymentConfiguration.Cvc,
                FromValidThroughMonth = _configuration.PaymentConfiguration.ValidThroughMonth,
                FromValidThroughYear = _configuration.PaymentConfiguration.ValidThroughYear,
                Sum = (int)(booking.Amount * booking.Tour.Price * discount),
                ToCardNumber = 0
            });
        }

        booking.Status = await _db.BookingStatuses.Where(bs => bs.Name == "Отменено").FirstAsync();
        booking.Tour.Leftover += booking.Amount;
        await _db.SaveChangesAsync();

        return RedirectPermanent("/Tours");
    }

    public async Task<IActionResult> PayForBooking(int bookingId)
    {
        var booking = await _db.Bookings.Where(b => b.Id == bookingId)
            .Include(b => b.Tour)
            .Include(b => b.User)
            .FirstAsync();

        var discount = 1.0;

        if ((booking.Tour.StartDate.ToDateTime(TimeOnly.MinValue) - DateTime.Now).Days < 14)
        {
            discount = 0.8;
        }

        var tourPrice = (int)(booking.Amount * booking.Tour.Price * discount);

        await _client.TransferCard(new TransferBody
        {
            FromCardNumber = 0,
            FromCvc = 0,
            FromValidThroughMonth = 0,
            FromValidThroughYear = 0,
            Sum = tourPrice,
            ToCardNumber = _configuration.PaymentConfiguration.CardNumber
        });
        
        booking.Status = await _db.BookingStatuses.Where(bs => bs.Name == "Оплачено").FirstAsync();
        await _db.SaveChangesAsync();

        return RedirectPermanent("/Tours");
    }
}
