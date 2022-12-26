using Microsoft.EntityFrameworkCore;
using PaymentAPI.Models;

namespace PaymentAPI.Data;

public class ApiDbContext : DbContext
{
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<CardStatus> CardStatus => Set<CardStatus>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(
            entity => { entity.HasIndex(e => e.CardNumber).IsUnique(); });
    }
}