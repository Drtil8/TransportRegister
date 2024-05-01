using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
    public class DriversLicenseSeed
    {
        public static void Seed(AppDbContext context)
        {
            var licensesToSeed = new DriversLicense[]
            {
                // Trucks
                new()
                {
                    VehicleType = VehicleType.C,
                    DriverId = 7,
                    Description = "Řidičský průkaz na náklaďák",
                    IssuedOn = DateOnly.Parse("2024-04-01"),
                },
                new()
                {
                    VehicleType = VehicleType.C,
                    DriverId = 8,
                    Description = "Řidičský průkaz na náklaďák",
                    IssuedOn = DateOnly.Parse("2024-03-10"),
                },

                // Buses
                new()
                {
                    VehicleType = VehicleType.D,
                    DriverId = 9,
                    Description = "Řidičský průkaz na autobus",
                    IssuedOn = DateOnly.Parse("2024-06-16"),
                },
                new()
                {
                    VehicleType = VehicleType.D,
                    DriverId = 10,
                    Description = "Řidičský průkaz na autobus",
                    IssuedOn = DateOnly.Parse("2024-06-16"),
                },
                new()
                {
                    VehicleType = VehicleType.D,
                    DriverId = 11,
                    Description = "Řidičský průkaz na autobus",
                    IssuedOn = DateOnly.Parse("2024-08-21"),
                },
              
                // Cars
                new()
                {
                    VehicleType = VehicleType.B,
                    DriverId = 7,
                    Description = "Řidičský průkaz na auto",
                    IssuedOn = DateOnly.Parse("2022-02-11"),
                },
                new()
                {
                    VehicleType = VehicleType.B,
                    DriverId = 8,
                    Description = "Řidičský průkaz na auto",
                    IssuedOn = DateOnly.Parse("2014-05-16"),
                },

                // Motorcycles
                new()
                {
                    VehicleType = VehicleType.A,
                    DriverId = 9,
                    Description = "Řidičský průkaz na motorku",
                    IssuedOn = DateOnly.Parse("2022-08-21"),
                },
                new()
                {
                    VehicleType = VehicleType.A,
                    DriverId = 10,
                    Description = "Řidičský průkaz na motorku",
                    IssuedOn = DateOnly.Parse("2022-08-11"),
                },
                new()
                {
                    VehicleType = VehicleType.A,
                    DriverId = 11,
                    Description = "Řidičský průkaz na motorku",
                    IssuedOn = DateOnly.Parse("2024-02-21"),
                }

            };
            foreach (var license in licensesToSeed)
            {
                context.DriversLicenses.Add(license);
                context.SaveChanges();
            }
        }

    }
}
