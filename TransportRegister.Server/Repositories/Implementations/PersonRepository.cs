using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using System;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Theft>> GetPersonReportedTheftsByIdAsync(int personId)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(v => v.PersonId == personId);
            if (person is null)
            {
                return null;
            }

            return await _context.Thefts
                .Include(t => t.StolenVehicle)
                    .ThenInclude(vehicle => vehicle.LicensePlates)
                .Where(t => t.ReportingPersonId == personId)
                .ToListAsync();
        }
        public async Task<List<Offence>> GetPersonCommitedOffencesByIdAsync(int personId)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(v => v.PersonId == personId);
            if (person is null)
            {
                return null;
            }

            return await _context.Offences
                .Include(o => o.Fine)
                .Include(o => o.OffenceType)
                .Where(o => o.PersonId == personId)
                .ToListAsync();
        }

        public async Task<Tuple<List<PersonSimpleListDto>,List<DriverSimpleListDto>>> GetAllPersons()
        {
            var query = from person in _context.Persons
                        join driver in _context.Drivers
                        on person.PersonId equals driver.PersonId into driverJoin
                        from driverData in driverJoin.DefaultIfEmpty()
                        select new
                        {
                            Person = person,
                            Driver = driverData,
                            DriverLicenses = driverData != null ? driverData.Licenses : null,
                        };

            var personDtos = new List<PersonSimpleListDto>();
            var driverDtos = new List<DriverSimpleListDto>();

            foreach (var tuple in await query.ToListAsync())
            {
                if (tuple.Driver != null)
                {
                    var driverDto = new DriverSimpleListDto
                    {
                        DriversLicenseNumber = tuple.Driver.DriversLicenseNumber,
                        PersonId = tuple.Person.PersonId,
                        FirstName = tuple.Person.FirstName,
                        LastName = tuple.Person.LastName,
                        BirthNumber = tuple.Person.BirthNumber,
                    };
                    driverDtos.Add(driverDto);
                }
                else 
                {
                    var personDto = new PersonSimpleListDto
                    {
                        PersonId = tuple.Person.PersonId,
                        FirstName = tuple.Person.FirstName,
                        LastName = tuple.Person.LastName,
                        BirthNumber = tuple.Person.BirthNumber,
                    // Add other common properties here
                    };
                    personDtos.Add(personDto); 
                }

            }
            return Tuple.Create(personDtos, driverDtos);
        }


        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            var person = await _context.Persons
                            .Include(owner => owner.Vehicles)
                            .ThenInclude(vehicle => vehicle.LicensePlates)
                            .FirstOrDefaultAsync(v => v.PersonId == personId);
            if (person is null)
            {
                return null;
            }

            if (person.GetType() == typeof(Driver))
            {
                return await _context.Drivers
                    .Include(v => v.Licenses)
                    .Include(owner => owner.Vehicles)
                        .ThenInclude(vehicle => vehicle.LicensePlates)
                    .FirstOrDefaultAsync(v => v.PersonId == personId);
            }
            else return person;
        }
        public async Task<Driver> GetDriverAsync(string licenseNumber)
        {
            return await _context.Drivers

                .FirstOrDefaultAsync(v => v.DriversLicenseNumber == licenseNumber);
        }
        public async Task<Person> GetOwnerByVINAsync(string VIN_number)
        {
            return await _context.Persons
                .Include(o => o.Vehicles)
                .FirstOrDefaultAsync(o => o.Vehicles.Any(v => v.VIN == VIN_number));
        }

        public async Task SetDriverAsync(Person driver)
        {
            if (driver.PersonId == 0)
            {
                _context.Drivers.Add(driver as Driver);
            }
            else
            {
                //driver.PersonType = "Driver";
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(int personId)
        {
            Person person = await _context.Persons.FirstOrDefaultAsync(d => d.PersonId == personId);
            if (person != null)
            {
                _context.Persons.Remove(person);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddDriversLicense(int driverId, DriversLicenseCreateDto license)
        {
            Driver driver = await _context.Drivers
                .Include(v => v.Licenses)
                .FirstOrDefaultAsync(d => d.PersonId == driverId);
            if (driver != null)
            {
                DriversLicense new_license = new DriversLicense
                {
                    DriversLicenseId = default,
                    IssuedOn = license.IssuedOn,
                    Description = license.Description,
                    VehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), license.VehicleType),
                    DriverId = driverId,
                };

                driver.Licenses.Add(new_license);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SavePersonAsync(Person person)
        {
            if (person.PersonId == default)
            {
                _context.Persons.Add(person);
            }
            else
            {
                _context.Persons.Update(person);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Tuple<Driver, int>>> GetDriversAndPoints()
        {
            var driversWithPoints = await _context.Drivers
                .Select(driver => new { Driver = driver, Points = driver.BadPoints }) // Select drivers and their points
                .ToListAsync();

            var result = driversWithPoints
                .Select(item => Tuple.Create(item.Driver, item.Points))
                .ToList();

            return result;
        }
    }
}
