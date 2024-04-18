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
                new()
                {
                    VehicleId= 1,
                    LicensePlate = "NR123RN",
                    ChangedOn = DateTime.Now,
                }
            };
            foreach (var licensePlate in platesToSeed)
            {
                context.LicensePlates.Add(licensePlate);
                context.SaveChanges();
            }
        }
    }
}
