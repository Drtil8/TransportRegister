using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
    public class TheftSeed
    {
        public static void Seed(AppDbContext context)
        {
            var theftsToSeed = new Theft[]
            {
                new()
                {
                    StolenOn = DateTime.Parse("2024-02-03"),
                    ReportedOn = DateTime.Parse("2024-02-03"),
                    FoundOn = DateTime.Parse("2024-02-27"),
                    Description = "Krádež motorového vozidla",
                    VehicleId = 1,
                    ReportingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    ResolvingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    ReportingPersonId = 1
                },
                new()
                {
                    StolenOn = DateTime.Parse("2024-03-15"),
                    ReportedOn = DateTime.Parse("2024-03-16"),
                    FoundOn = null,
                    Description = "Krádež2",
                    VehicleId = 3,
                    ReportingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    ResolvingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    ReportingPersonId = 1
                }
            };
            foreach (var theft in theftsToSeed)
            {
                if (!context.Thefts.Any(o => o.TheftId == theft.TheftId))
                {
                    context.Thefts.Add(theft);
                }
            }
            context.SaveChanges();
        }
    }
}
