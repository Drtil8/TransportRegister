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
                    OffenceId = 1,
                    Amount= 1000,
                    IsActive = false, // Paid
                    DueDate = DateOnly.Parse("2024-04-11"),
                    PaidOn = DateOnly.Parse("2024-04-12"),
                },
                new()
                {
                    OffenceId = 2,
                    Amount= 500,
                    IsActive = true, // Not yet paid
                    DueDate = DateOnly.Parse("2021-05-11"),
                },
                new()
                {
                    OffenceId = 3,
                    Amount= 2000,
                    IsActive = true, // Not yet paid
                    DueDate = DateOnly.Parse("2022-05-08"),
                },

            };
            foreach (var fine in finesToSeed)
            {
                context.Fines.Add(fine);
                context.SaveChanges();
            }
        }
    }
}
