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
                    LicensePlate = "3A4 567",
                    ChangedOn = DateTime.Parse("2024-04-12"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 2,
                    LicensePlate = "5B6 781",
                    ChangedOn = DateTime.Parse("2017-08-22"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 3,
                    LicensePlate = "9C7 234",
                    ChangedOn = DateTime.Parse("2022-01-05"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 4,
                    LicensePlate = "2T7 623",
                    ChangedOn = DateTime.Parse("2023-12-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 5,
                    LicensePlate = "2J4 987",
                    ChangedOn = DateTime.Parse("2022-12-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 6,
                    LicensePlate = "7J3 7700",
                    ChangedOn = DateTime.Parse("2023-11-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 7,
                    LicensePlate = "5A6 4652",
                    ChangedOn = DateTime.Parse("2013-02-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 8,
                    LicensePlate = "6AA 1325",
                    ChangedOn = DateTime.Parse("2020-10-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 9,
                    LicensePlate = "2U2 4820",
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
                    LicensePlate = "5B2 9841",
                    ChangedOn = DateTime.Parse("2013-02-01"),
                },
                new LicensePlateHistory()
                {
                    VehicleId = 12,
                    LicensePlate = "8B2 8453",
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
