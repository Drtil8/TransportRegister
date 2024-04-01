using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public class OwnerSeed
{
    public static void Seed(AppDbContext context)
    {
        if (!context.Owners.Any())
        {
            var owner1Id = Guid.Parse("12345678-1234-1234-1234-1234567890ab");
            var owner2Id = Guid.Parse("87654321-4321-4321-4321-ba0987654321"); 

            var owner1 = new Owner
            {
                OwnerId = owner1Id,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var owner2 = new Owner
            {
                OwnerId = owner2Id,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com"
            };

            context.Owners.AddRange(owner1, owner2);
            context.SaveChanges();
        }
    }
}