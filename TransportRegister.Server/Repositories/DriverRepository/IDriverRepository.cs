using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.DriverRepository
{
    public interface IDriverRepository
    {
        Task<Driver> GetDriverAsync(string licenseNumber);
        Task SaveDriverAsync(Driver driver);
        Task DeleteDriverAsync(int driverId);
        Task<List<Tuple<Driver, int>>> GetDriversAndPoints();
        Task<Driver> GetDriverByIdAsync(int driverId);
    }
}
