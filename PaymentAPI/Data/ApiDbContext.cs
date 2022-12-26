using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;

namespace PaymentAPI.Data;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }
}