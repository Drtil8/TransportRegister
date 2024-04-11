using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
namespace TransportRegister.Server.Seeds
{
    public class OffenceSeeds
    {
        public static void Seed(AppDbContext context)
        {
            var offences = new Offence[]
            {
                new()
                {
                    ReportedOn = DateTime.Now,
                    IsValid= true,
                    FineId = null,
                    VehicleId = Guid.Parse("add"),
                    OfficerId = "A",
                    OfficialId = "B",
                    PersonId= Guid.Parse("add")



                }
            };
        }
    }
}
