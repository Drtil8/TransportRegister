using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) {}

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Owner> Owners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Owner>()
            .HasMany(o => o.Vehicles)
            .WithOne(v => v.Owner)
            .HasForeignKey(v => v.OwnerId);
    }
}