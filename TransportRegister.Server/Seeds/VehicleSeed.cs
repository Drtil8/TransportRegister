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
                new Vehicle { Make = "Toyota", Model = "Corolla", Year = 2020 },
                new Vehicle { Make = "Honda", Model = "Civic", Year = 2019 }
            );
            context.SaveChanges();
        }
    }
}