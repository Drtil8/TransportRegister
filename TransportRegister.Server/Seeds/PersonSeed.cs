using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using System;

namespace TransportRegister.Server.Seeds
{
    public class PersonSeed
    {
        public static void Seed(AppDbContext context)
        {
            var ownersToSeed = new Person[]
            {
                new()
                {
                    FirstName = "Jiøí",
                    LastName = "Kostka",
                    BirthNumber = "0208124356",
                    DateOfBirth = DateOnly.Parse("2020-08-12"),
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
                    Image = null,
                    Sex_Male = true
                },
                new()
                {
                    FirstName = "Lucie",
                    LastName = "Kolarczyková",
                    BirthNumber = "9456154103",
                    DateOfBirth = DateOnly.Parse("1994-06-15"),
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
                    Image = null,
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "David",
                    LastName = "Drtil",
                    BirthNumber = "7811010933",
                    DateOfBirth = DateOnly.Parse("1978-11-01"),
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
                    Image = null,
                    Sex_Male = true
                },

                new()
                {
                    FirstName = "Andrea",
                    LastName = "Jehlíková",
                    BirthNumber = "9453154103",
                    DateOfBirth = DateOnly.Parse("1994-03-15"),
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
                    Image = null,
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "Marie",
                    LastName = "Rejtharová",
                    BirthNumber = "0154248490",
                    DateOfBirth = DateOnly.Parse("2001-04-24"),
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
                    Image = null,
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "Karel",
                    LastName = "Bušek",
                    BirthNumber = "7708219629",
                    DateOfBirth = DateOnly.Parse("1977-08-21"),
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
                    Image = null,
                    Sex_Male = true
                }
            };

            var driversToSeed = new Driver[]
            {
                new()
                {
                    FirstName = "Petr",
                    LastName = "Jetmar",
                    BirthNumber = "0109067739",
                    DateOfBirth = DateOnly.Parse("2001-09-06"),
                    DriversLicenseNumber ="ABC1234",
                    BadPoints = 5,
                    HasSuspendedLicense = true,
                    DrivingSuspendedUntil = DateTime.Parse("2025-12-12"),
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
                    Image = null,
                    Sex_Male = true
                },
                new()
                {
                    FirstName = "Denisa",
                    LastName = "Macháèková",
                    BirthNumber = "9460087670",
                    DateOfBirth = DateOnly.Parse("1994-10-08"),
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
                    Image = null,
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "David",
                    LastName = "Páleník",
                    BirthNumber = "9103157756",
                    DateOfBirth = DateOnly.Parse("1991-03-15"),
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
                    Image = null,
                    Sex_Male = true
                },

                new()
                {
                    FirstName = "Ludmila",
                    LastName = "Benková",
                    BirthNumber = "7660052081",
                    DateOfBirth = DateOnly.Parse("1976-10-05"),
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
                    Image = null,
                    Sex_Male = false
                },

                new()
                {
                    FirstName = "Pavel",
                    LastName = "Labaè",
                    BirthNumber = "8208126124",
                    DateOfBirth = DateOnly.Parse("1982-08-12"),
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
                    Image = null,
                    Sex_Male = true
                }
            };

            foreach (var owner in ownersToSeed)
            {
                if (!context.Persons.Any(o => o.PersonId == owner.PersonId))
                {
                    context.Persons.Add(owner);
                }
            }
            context.SaveChanges();

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
