using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public class VehicleSeed
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Vehicles.Any())
        {
            context.Vehicles.AddRange(
                new Vehicle 
                { 
                    Make = "Toyota", 
                    Model = "Corolla", 
                    Year = 2020, 
                    OwnerId = Guid.Parse("12345678-1234-1234-1234-1234567890ab") 
                },
                new Vehicle 
                { 
                    Make = "Honda", 
                    Model = "Civic", 
                    Year = 2019, 
                    OwnerId = Guid.Parse("87654321-4321-4321-4321-ba0987654321") 
                },
                new Vehicle 
                { 
                    Make = "Ford", 
                    Model = "Focus", 
                    Year = 2021, 
                    OwnerId = Guid.Parse("12345678-1234-1234-1234-1234567890ab")
                }
            );
            context.SaveChanges();
        }
    }
}