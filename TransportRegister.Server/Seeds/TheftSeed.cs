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
                    ReturnedOn = DateTime.Parse("2024-02-28"),
                    Description = "Krádež motorového vozidla -> uzavřený stav",
                    VehicleId = 1,
                    ReportingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    ResolvingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    ReportingPersonId = 6,
                    Address = new Address
                    {
                        Street = "Chodská",
                        City = "Brno-město",
                        State = "Jihomoravksý kraj",
                        HouseNumber = 19,
                        PostalCode = 612,
                        Country = "Česko"
                    }
                },
                new()
                {
                    StolenOn = DateTime.Parse("2024-03-15"),
                    ReportedOn = DateTime.Parse("2024-03-16"),
                    FoundOn = null,
                    ReturnedOn = null,
                    Description = "Krádež -> počáteční stav",
                    VehicleId = 2,
                    ReportingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    //ResolvingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    //OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    ReportingPersonId = 7,
                    Address = new Address
                    {
                        Street = "Čekyňská",
                        City = "Přerov",
                        State = "Olomoucký kraj",
                        HouseNumber = 52,
                        PostalCode = 712,
                        Country = "Česko"
                    }
                },
                new()
                {
                    StolenOn = DateTime.Parse("2024-03-15"),
                    ReportedOn = DateTime.Parse("2024-03-16"),
                    FoundOn = DateTime.Parse("2024-03-17"),
                    ReturnedOn = null,
                    Description = "Krádež -> prostřední stav",
                    VehicleId = 3,
                    ReportingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    ResolvingOfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    //OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    ReportingPersonId = 8,
                    Address = new Address
                    {
                        Street = "Velká Dlážka",
                        City = "Přerov",
                        State = "Olomoucký kraj",
                        HouseNumber = 2,
                        PostalCode = 712,
                        Country = "Česko"
                    }
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
