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
                new()
                {
                    OffenceId = Guid.Parse("87654321-1111-2222-3333-1234567890aa"),
                    ReportedOn = DateTime.Now,
                    IsValid= true,
                    //FineId = Guid.Empty, dajaka chyba nvm
                    VehicleId = Guid.Parse("87654321-1111-2222-3333-1234567890cd"),
                    OfficerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    PersonId= Guid.Parse("12345678-1234-1234-1234-1234567890ab")



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
