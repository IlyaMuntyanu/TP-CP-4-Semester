using Microsoft.EntityFrameworkCore;
using TP_CP_5_Semester.Models;

namespace TP_CP_5_Semester.Data;

public class ApplicationDbContext: DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingStatus> BookingStatuses => Set<BookingStatus>();
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
}