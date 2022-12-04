using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Models;

namespace TP_CP_5_Semester.Data;

public class ApplicationDbContext: DbContext
{
    public DbSet<User> Users;
    public DbSet<Role> Roles;
    public DbSet<Tour> Tours;
    public DbSet<Booking> Bookings;
    public DbSet<BookingStatus> BookingStatuses;
}