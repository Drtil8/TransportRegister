using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public class PersonSeed
{
    public static void Seed(AppDbContext context)
    {
        var ownersToSeed = new Owner[]
        {
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                BirthNumber = "ABCD12345",
                Address = new Address
                {
                    Street = "Sample Street",
                    City = "Sample City",
                    State = "Sample State",
                    Country = "Sample Country",
                    HouseNumber = 123,
                    PostalCode = 12345
                },
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"

            },
            new()
            {

                FirstName = "Jane",
                LastName = "Doe",
                BirthNumber = "ABCD11111",
                Address = new Address
                {
                    Street = "Nice Street",
                    City = "Nice City",
                    State = "Nice State",
                    Country = "Nice Country",
                    HouseNumber = 100,
                    PostalCode = 12345
                },
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
            }
        };

        var driversToSeed = new Driver[]
{
            new()
            {
                FirstName = "Joseph",
                LastName = "Driver",

                DriversLicenseNumber="ABC1234",
                BadPoints = 5,
                HasSuspendedLicense = false,
                LastCrimeCommited = DateTime.Now,

                BirthNumber = "ABCD99999",
                Address = new Address
                {
                    Street = "Bad Street",
                    City = "Bad City",
                    State = "Bad State",
                    Country = "Bad Country",
                    HouseNumber = 666,
                    PostalCode = 12345
                },
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
            }
        };

        foreach (var owner in ownersToSeed)
        {
            if (!context.Owners.Any(o => o.PersonId == owner.PersonId))
            {
                context.Owners.Add(owner);
            }
        }

        foreach (var driver in driversToSeed)
        {
            if (!context.Drivers.Any(o => o.PersonId == driver.PersonId))
            {
                context.Drivers.Add(driver);
            }
        }
        context.SaveChanges();
    }
}
