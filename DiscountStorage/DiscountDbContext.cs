using DiscountStorage.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountStorage;

public class DiscountDbContext : DbContext
{
    public DbSet<DiscountCode> DiscountCodes => Set<DiscountCode>();

    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiscountCode>()
            .HasIndex(dc => dc.Code)
            .IsUnique();
    }
}
