using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
    public class LicensePlateHistorySeed
    {
        public static void Seed(AppDbContext context)
        {
            var platesToSeed = new LicensePlateHistory[]
            {
                new LicensePlateHistory()
                {
                    VehicleId = 1,
                    LicensePlate = "1A2 3456",
                    ChangedOn = DateTime.Parse("2024-04-12"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 2,
                    LicensePlate = "1B2 3456",
                    ChangedOn = DateTime.Parse("2017-08-22"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 3,
                    LicensePlate = "1C2 3456",
                    ChangedOn = DateTime.Parse("2022-01-05"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 4,
                    LicensePlate = "1D2 3456",
                    ChangedOn = DateTime.Parse("2023-12-01"),
                },
            };
            foreach (var licensePlate in platesToSeed)
            {
                context.LicensePlates.Add(licensePlate);
                context.SaveChanges();
            }
        }
    }
}
