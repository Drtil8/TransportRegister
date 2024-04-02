using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Owner> Owners { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Owner>()
            .HasMany(o => o.Vehicles)
            .WithOne(v => v.Owner)
            .HasForeignKey(v => v.OwnerId);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Official>("Official")
                .HasValue<Officer>("Officer");
        });
    }
}