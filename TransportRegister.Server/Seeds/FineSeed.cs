using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
    public class FineSeed
    {
        public static void Seed(AppDbContext context)
        {
            var finesToSeed = new Fine[]
            {
                new()
                {
                    FineId = 1,
                    OffenceId = 1,
                    Amount= 5000,
                    PaidOn = DateOnly.Parse("2024-04-11"),
                    

                }
            };
            foreach (var fine in finesToSeed)
            {
                context.Fines.Add(fine);
                context.SaveChanges(); 

                
                var offence = context.Offences.FirstOrDefault(o => o.OffenceId == 1);
                if (offence != null)
                {
                    offence.FineId = fine.FineId;
                    context.SaveChanges(); 
                }
            }
        }
    }
}
