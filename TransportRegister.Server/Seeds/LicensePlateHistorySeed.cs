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
                new LicensePlateHistory()
                {
                    VehicleId = 5,
                    LicensePlate = "1E2 3456",
                    ChangedOn = DateTime.Parse("2022-12-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 6,
                    LicensePlate = "1F2 3456",
                    ChangedOn = DateTime.Parse("2023-11-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 7,
                    LicensePlate = "1G2 3456",
                    ChangedOn = DateTime.Parse("2013-02-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 8,
                    LicensePlate = "1H2 3456",
                    ChangedOn = DateTime.Parse("2020-10-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 9,
                    LicensePlate = "1I2 3456",
                    ChangedOn = DateTime.Parse("2023-10-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 10,
                    LicensePlate = "1J2 3456",
                    ChangedOn = DateTime.Parse("2015-10-06"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 11,
                    LicensePlate = "K2 3456",
                    ChangedOn = DateTime.Parse("2013-02-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 12,
                    LicensePlate = "1L2 3456",
                    ChangedOn = DateTime.Parse("2014-04-04"),
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
