using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
namespace TransportRegister.Server.Seeds
{
    public class OffenceSeed
    {
        public static void Seed(AppDbContext context)
        {
            var offencesToSeed = new Offence[]
            {
                new() // Approved offence with fine
                {
                    ReportedOn = DateTime.Parse("2024-04-07 12:00:00"),
                    IsValid = true,
                    IsApproved = true,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2,
                    OffenceTypeId = 2
                },
                new() // Yet not approved offence with fine
                {
                    ReportedOn = DateTime.Parse("2024-05-04 12:00:00"),
                    IsValid = true,
                    IsApproved = false,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    //OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2,
                    OffenceTypeId = 10
                },
                new() // Denied offence with fine
                {
                    ReportedOn = DateTime.Parse("2024-05-01 12:00:00"),
                    IsValid = false,
                    IsApproved = false,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2,
                    OffenceTypeId = 1
                },
                new() // Approved offence without fine
                {
                    ReportedOn = DateTime.Now,
                    IsValid = true,
                    IsApproved = true,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2,
                    OffenceTypeId = 4
                }
            };
            foreach (var offence in offencesToSeed)
            {
                if (!context.Offences.Any(o => o.OffenceId == offence.OffenceId))
                {
                    context.Offences.Add(offence);
                }
            }
            context.SaveChanges();
        }
    }
}
