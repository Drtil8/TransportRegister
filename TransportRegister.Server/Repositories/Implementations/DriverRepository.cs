using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories.DriverRepository;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class DriverRepository : IDriverRepository
    {
        private readonly AppDbContext _context;

        public DriverRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Driver> GetDriverAsync(string licenseNumber)
        {
            return await _context.Drivers

                .FirstOrDefaultAsync(v => v.DriversLicenseNumber == licenseNumber);
        }

        public async Task<Driver> GetDriverByIdAsync(int driverId)
        {
            return await _context.Drivers
                .Include(v => v.Licenses)
                .Include(v => v.CommitedOffences)
                .Include(v => v.ReportedThefts)
                .FirstOrDefaultAsync(v => v.PersonId == driverId);
        }

        public async Task SaveDriverAsync(Driver driver)
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

        public async Task DeleteDriverAsync(int driverId)
        {
            Driver driver = await _context.Drivers.FirstOrDefaultAsync(d => d.PersonId == driverId);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Tuple<Driver, int>>> GetDriversAndPoints()
        {
            var driversWithPoints = await _context.Drivers
                .Select(driver => new { Driver = driver, Points = driver.BadPoints }) // Select drivers and their points
                .ToListAsync();

            // Convert to DTO ??
            var result = driversWithPoints
                .Select(item => Tuple.Create(item.Driver, item.Points))
                .ToList();

            return result;
        }

    }
}
