using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Person> GetPersonByIdAsync(int personId)
        {
            return await _context.Persons
                //.Include(v => v.CommitedOffences)
                //.Include(v => v.ReportedThefts)
                .FirstOrDefaultAsync(v => v.PersonId == personId);
        }
        public async Task<Driver> GetDriverAsync(string licenseNumber)
        {
            return await _context.Drivers

                .FirstOrDefaultAsync(v => v.DriversLicenseNumber == licenseNumber);
        }
        public async Task<Owner> GetOwnerByVINAsync(string VIN_number)
        {
            return await _context.Owners
                .Include(o => o.Vehicles)
                .FirstOrDefaultAsync(o => o.Vehicles.Any(v => v.VIN == VIN_number));
        }

        public async Task SetOwnerAsync(Owner owner)
        {
            if (owner.PersonId == 0)
            {
                _context.Owners.Add(owner);
            }
            else
            {
                _context.Owners.Update(owner);
            }
            await _context.SaveChangesAsync();
        }


        public async Task SetDriverAsync(Driver driver)
        {
            if (driver.PersonId == 0)
            {
                _context.Drivers.Add(driver);
            }
            else
            {
                _context.Drivers.Update(driver);
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
