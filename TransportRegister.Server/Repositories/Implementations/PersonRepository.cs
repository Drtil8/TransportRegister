using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.DatatableDTOs;
using System;

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

        public async Task<Person> GetPersonByBirthNumberAsync(string birthNumber)
        {
            var person = await _context.Persons
                                       .Include(owner => owner.Vehicles)
                                       .ThenInclude(vehicle => vehicle.LicensePlates)
                                       .FirstOrDefaultAsync(v => v.BirthNumber == birthNumber);
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
                    .FirstOrDefaultAsync(v => v.BirthNumber == birthNumber);
            }
            else return person;
        }

        public async Task AddDriverAsync(int personId, string license)
        {

            var newDriver = new Driver
            {
                PersonId = personId,
                DriversLicenseNumber = license,
                BadPoints = 0,
                HasSuspendedLicense = false,
                LastCrimeCommited = null,
                DrivingSuspendedUntil = null
            };

            // Add the new driver to the context
            _context.Drivers.Add(newDriver);

            // Save changes to the database
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
        public async Task AddDriversLicense(int driverId,string official_id,  DriversLicenseCreateDto license)
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
                driver.OfficialId = official_id;
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

        public async Task SaveDriverAsync(Driver driver)
        {
            if (driver.PersonId == default)
            {
                _context.Drivers.Add(driver);
            }
            else
            {
                _context.Drivers.Update(driver);
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


        // Person search

        private static IQueryable<PersonSimpleListDto> ApplySortingPersonSearch(
    IQueryable<PersonSimpleListDto> query, DtParamsDto dtParams)
        {
            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                return query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(v => v.PersonId);
            }
            else
            {
                return query.OrderByDescending(v => v.PersonId);
            }
        }

        private static IQueryable<PersonSimpleListDto> ApplyFilterPersonSearch(
            IQueryable<PersonSimpleListDto> query, DtParamsDto dtParams)
        {
            foreach (var filter in dtParams.Filters)
            {
                // todo string properties can be filtered by Contains or StartsWith
                query = filter.PropertyName switch
                {
                    nameof(PersonSimpleListDto.PersonId) =>
                        query.Where(v => v.PersonId.ToString().StartsWith(filter.Value)), // numeric property
                    nameof(PersonSimpleListDto.FirstName) =>
                        query.Where(v => v.FirstName.StartsWith(filter.Value)),
                    nameof(PersonSimpleListDto.LastName) =>
                        query.Where(v => v.LastName.StartsWith(filter.Value)),
                    nameof(PersonSimpleListDto.BirthNumber) =>
                        query.Where(v => v.BirthNumber.StartsWith(filter.Value)),
                    _ => query      // Default case - do not apply any filter    // Default case - do not apply any filter
                };
            }
            return query;
        }

        // Driver Search
        private static IQueryable<DriverSimpleListDto> ApplySortingDriverSearch(
        IQueryable<DriverSimpleListDto> query, DtParamsDto dtParams)
        {
            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                return query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(v => v.PersonId);
            }
            else
            {
                return query.OrderByDescending(v => v.PersonId);
            }
        }

        private static IQueryable<DriverSimpleListDto> ApplyFilterDriverSearch(
            IQueryable<DriverSimpleListDto> query, DtParamsDto dtParams)
        {
            foreach (var filter in dtParams.Filters)
            {
                // todo string properties can be filtered by Contains or StartsWith
                query = filter.PropertyName switch
                {
                    nameof(DriverSimpleListDto.PersonId) =>
                        query.Where(v => v.PersonId.ToString().StartsWith(filter.Value)), // numeric property
                    nameof(DriverSimpleListDto.DriversLicenseNumber) =>
                        query.Where(v => v.DriversLicenseNumber.StartsWith(filter.Value)),
                    nameof(DriverSimpleListDto.FirstName) =>
                        query.Where(v => v.FirstName.StartsWith(filter.Value)),
                    nameof(DriverSimpleListDto.LastName) =>
                        query.Where(v => v.LastName.StartsWith(filter.Value)),
                    nameof(DriverSimpleListDto.BirthNumber) =>
                        query.Where(v => v.BirthNumber.StartsWith(filter.Value)),
                    _ => query      // Default case - do not apply any filter
                };
            }
            return query;
        }

        public IQueryable<DriverSimpleListDto> QueryAllPersons(DtParamsDto dtParams)
        {
            var query = from person in _context.Persons
                join driver in _context.Drivers
                on person.PersonId equals driver.PersonId into driverJoin
                from driverData in driverJoin.DefaultIfEmpty()
                select new DriverSimpleListDto
                {
                    PersonId = person.PersonId,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    BirthNumber = person.BirthNumber,
                    DriversLicenseNumber = driverData != null ? driverData.DriversLicenseNumber : null
                };
            query = ApplyFilterDriverSearch(query, dtParams);
            query = ApplySortingDriverSearch(query, dtParams);
            return query;
        }
    }
}
