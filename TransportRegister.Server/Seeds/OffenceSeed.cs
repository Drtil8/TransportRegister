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
                new() // Approved offence without fine
                {
                    ReportedOn = DateTime.Now,
                    IsValid = true,
                    IsApproved = true,
                    //FineId = null,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2
                },
                new() // Yet not approved offence without fine
                {
                    ReportedOn = DateTime.Now,
                    IsValid = true,
                    IsApproved = false,
                    //FineId = null,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2
                },
                new() // Denied offence without fine
                {
                    ReportedOn = DateTime.Now,
                    IsValid = false,
                    IsApproved = false,
                    //FineId = null,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2
                },
                new() // Approved offence with fine
                {
                    ReportedOn = DateTime.Now,
                    IsValid = true,
                    IsApproved = true,
                    //FineId = 1,
                    VehicleId = 1,
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId = 2
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
