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
                    StolenOn = DateTime.Now,
                    ReportedOn = DateTime.Now,
                    FoundOn = DateTime.Now,
                    Description= "Krádež motorového vozidla",

                    VehicleId = Guid.Parse("87654321-1111-2222-3333-1234567890cd"),
                    ReportingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    ResolvingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    ReportingPersonId= Guid.Parse("12345678-1234-1234-1234-1234567890ab")



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
