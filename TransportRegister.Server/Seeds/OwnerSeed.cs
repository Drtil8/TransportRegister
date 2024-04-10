using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public class OwnerSeed
{
    public static void Seed(AppDbContext context)
    {
        var ownersToSeed = new Owner[]
        {
            new()
            {
                PersonId = Guid.Parse("12345678-1234-1234-1234-1234567890ab"),
                FirstName = "John",
                LastName = "Doe",
                //Email = "john.doe@example.com"
            },
            new()
            {
                PersonId = Guid.Parse("87654321-4321-4321-4321-ba0987654321"),
                FirstName = "Jane",
                LastName = "Doe",
                //Email = "jane.doe@example.com"
            }
        };

        foreach (var owner in ownersToSeed)
        {
            if (!context.Owners.Any(o => o.PersonId == owner.PersonId))
            {
                context.Owners.Add(owner);
            }
        }
        context.SaveChanges();
    }
}
