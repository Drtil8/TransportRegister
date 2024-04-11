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
                    OffenceId = Guid.Parse("87654321-1111-2222-3333-1234567890aa"),
                    Amount= 5000,
                    PaidOn = DateOnly.Parse("2024-04-11"),
                    

                }
            };
            foreach (var fine in finesToSeed)
            {
                context.Fines.Add(fine);
                context.SaveChanges(); // Save changes after adding each fine

                // Find the corresponding offence and update its FineId
                var offence = context.Offences.FirstOrDefault(o => o.OffenceId == Guid.Parse("87654321-1111-2222-3333-1234567890aa"));
                if (offence != null)
                {
                    offence.FineId = fine.FineId;
                    context.SaveChanges(); // Save changes after updating the offence
                }
            }
        }
    }
}
