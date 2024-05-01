using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using System;

namespace TransportRegister.Server.Seeds
{
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
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = true
                },
                new()
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    BirthNumber = "EFGH67890",
                    Address = new Address
                    {
                        Street = "Main Street",
                        City = "Big City",
                        State = "Big State",
                        Country = "Big Country",
                        HouseNumber = 456,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "David",
                    LastName = "Drtil",
                    BirthNumber = "WXYZ54321",
                    Address = new Address
                    {
                        Street = "Green Street",
                        City = "Green City",
                        State = "Green State",
                        Country = "Green Country",
                        HouseNumber = 789,
                        PostalCode = 67890
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stig.jpg"),
                    Sex_Male = true
                },

                new()
                {
                    FirstName = "Emma",
                    LastName = "Brown",
                    BirthNumber = "JKLM12345",
                    Address = new Address
                    {
                        Street = "Brown Street",
                        City = "Brown City",
                        State = "Brown State",
                        Country = "Brown Country",
                        HouseNumber = 321,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "Sophia",
                    LastName = "Taylor",
                    BirthNumber = "UVWX54321",
                    Address = new Address
                    {
                        Street = "Red Street",
                        City = "Red City",
                        State = "Red State",
                        Country = "Red Country",
                        HouseNumber = 456,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "William",
                    LastName = "Anderson",
                    BirthNumber = "LMNO98765",
                    Address = new Address
                    {
                        Street = "Anderson Street",
                        City = "Anderson City",
                        State = "Anderson State",
                        Country = "Anderson Country",
                        HouseNumber = 789,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = true
                }
            };

            var driversToSeed = new Driver[]
            {
                new()
                {
                    FirstName = "Joseph",
                    LastName = "Driver",
                    BirthNumber = "ABCD99999",
                    DriversLicenseNumber ="ABC1234",
                    BadPoints = 5,
                    HasSuspendedLicense = false,
                    LastCrimeCommited = DateTime.Now,
                    LastPointsDeduction = DateTime.Now,
                    Address = new Address
                    {
                        Street = "Bad Street",
                        City = "Bad City",
                        State = "Bad State",
                        Country = "Bad Country",
                        HouseNumber = 666,
                        PostalCode = 12345
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = true
                },
                new()
                {
                    FirstName = "Emily",
                    LastName = "Driver",
                    BirthNumber = "EFGH12345",
                    DriversLicenseNumber ="DEF5678",
                    BadPoints = 3,
                    HasSuspendedLicense = false,
                    LastCrimeCommited = DateTime.Now,
                    LastPointsDeduction = DateTime.Now,
                    Address = new Address
                    {
                        Street = "Good Street",
                        City = "Good City",
                        State = "Good State",
                        Country = "Good Country",
                        HouseNumber = 123,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "David",
                    LastName = "Brown",
                    BirthNumber = "PQRS67890",
                    DriversLicenseNumber ="GHI5678",
                    BadPoints = 2,
                    HasSuspendedLicense = false,
                    LastCrimeCommited = DateTime.Now,
                    LastPointsDeduction = DateTime.Now,
                    Address = new Address
                    {
                        Street = "Yellow Street",
                        City = "Yellow City",
                        State = "Yellow State",
                        Country = "Yellow Country",
                        HouseNumber = 321,
                        PostalCode = 98765
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = true
                },

                new()
                {
                    FirstName = "Olivia",
                    LastName = "Johnson",
                    BirthNumber = "QRST98765",
                    DriversLicenseNumber ="JKL5678",
                    BadPoints = 4,
                    HasSuspendedLicense = false,
                    LastCrimeCommited = DateTime.Now,
                    LastPointsDeduction = DateTime.Now,
                    Address = new Address
                    {
                        Street = "Blue Street",
                        City = "Blue City",
                        State = "Blue State",
                        Country = "Blue Country",
                        HouseNumber = 456,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "Ethan",
                    LastName = "Williams",
                    BirthNumber = "IJKL54321",
                    DriversLicenseNumber ="STU5678",
                    BadPoints = 1,
                    HasSuspendedLicense = false,
                    LastCrimeCommited = DateTime.Now,
                    LastPointsDeduction = DateTime.Now,
                    Address = new Address
                    {
                        Street = "White Street",
                        City = "White City",
                        State = "White State",
                        Country = "White Country",
                        HouseNumber = 789,
                        PostalCode = 54321
                    },
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Drivers/stickman.png"),
                    Sex_Male = true
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
}
