using Microsoft.AspNetCore.Identity;

namespace TP_CP_5_Semester.Models;

public class Booking
{
    public int Id { get; set; }
    public IdentityUser User { get; set; }
    public Tour Tour { get; set; }
    public int Amount { get; set; }
    public BookingStatus Status { get; set; }
}