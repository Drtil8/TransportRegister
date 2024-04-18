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
                new()
                {
                    VehicleType = VehicleType.Bus,
                    DriverId = 1,
                    Description = "Vodičák na autobus",
                    IssuedOn = DateOnly.Parse("2024-04-11"),
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
