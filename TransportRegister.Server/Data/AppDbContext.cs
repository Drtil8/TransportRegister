using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Owner> Owners { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriversLicense> DriversLicenses { get; set; }
    public DbSet<Fine> Fines { get; set; }
    public DbSet<LicensePlateHistory> LicensePlates { get; set; }
    public DbSet<Offence> Offences { get; set; }
    public DbSet<Theft> Thefts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Owner
        modelBuilder.Entity<Owner>()
            .HasMany(o => o.Vehicles)
            .WithOne(v => v.Owner)
            .HasForeignKey(v => v.OwnerId);

        // Driver
        modelBuilder.Entity<Driver>()
            .HasMany(d => d.Licenses)
            .WithOne(v => v.IssuedFor)
            .HasForeignKey(v => v.DriverId)
            .IsRequired(true);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasDiscriminator<string>("UserType")
                .HasValue<User>("User")
                .HasValue<Official>("Official")
                .HasValue<Officer>("Officer");
        });

        // Vehicle
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("Vehicles");
            entity.HasDiscriminator<string>("VehicleType")
                .HasValue<Vehicle>("VehicleBase")
                .HasValue<Car>("Car")
                .HasValue<Truck>("Truck")
                .HasValue<Motorcycle>("Motorcycle")
                .HasValue<Bus>("Bus");
        });

        modelBuilder.Entity<Vehicle>()
            .HasMany(o => o.Offences)
            .WithOne(v => v.OffenceOnVehicle)
            .HasForeignKey(v => v.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Vehicle>()
            .HasMany(t => t.Thefts)
            .WithOne(v => v.StolenVehicle)
            .HasForeignKey(i => i.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Vehicle>()
            .HasMany(l => l.LicensePlates)
            .WithOne(v => v.Vehicle)
            .HasForeignKey(i => i.VehicleId);

        // Official
        modelBuilder.Entity<Official>()
            .HasMany(a => a.AddedVehicles)
            .WithOne(o => o.AddedByOfficial)
            .HasForeignKey("OfficialId");

        modelBuilder.Entity<Official>()
            .HasMany(p => p.AddedPersons)
            .WithOne(o => o.AddedByOfficial)
            .HasForeignKey(i => i.OfficialId);

        modelBuilder.Entity<Official>()
            .HasMany(t => t.ProcessedThefts)
            .WithOne(o => o.ProcessedByOfficial)
            .HasForeignKey(i => i.OfficialId);

        modelBuilder.Entity<Official>()
            .HasMany(p => p.ProcessedOffences)
            .WithOne(o => o.ProcessedByOfficial)
            .HasForeignKey(i => i.OfficialId)
            .OnDelete(DeleteBehavior.NoAction);

        // Officer
        modelBuilder.Entity<Officer>()
            .HasMany(r => r.ReportedOffences)
            .WithOne(o => o.ReportedByOfficer)
            .HasForeignKey(i => i.OfficerId);

        modelBuilder.Entity<Officer>()
            .HasMany(r => r.ReportedThefts)
            .WithOne(o => o.ReportedByOfficer)
            .HasForeignKey(i => i.ReportingOfficerId);

        modelBuilder.Entity<Officer>()
            .HasMany(r => r.ResolvedThefts)
            .WithOne(o => o.ResolvedByOfficer)
            .HasForeignKey(i => i.ResolvingOfficerId);

        // Person
        modelBuilder.Entity<Person>()
            .HasMany(o => o.CommitedOffences)
            .WithOne(c => c.CommitedBy)
            .HasForeignKey(i => i.PersonId);

        modelBuilder.Entity<Person>()
            .HasMany(t => t.ReportedThefts)
            .WithOne(p => p.ReportedByPerson)
            .HasForeignKey(i => i.ReportingPersonId);

        // Offence
        modelBuilder.Entity<Offence>()
            .HasOne(o => o.Fine)
            .WithOne(c => c.IssuedFor)
            .HasForeignKey<Fine>(i => i.FineId);
    }
}
