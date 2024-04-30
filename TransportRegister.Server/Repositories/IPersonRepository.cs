using TransportRegister.Server.Models;
namespace TransportRegister.Server.Repositories
{
    public interface IPersonRepository
    {

        Task<Person> GetPersonByIdAsync(int personId);
        Task<Driver> GetDriverAsync(string licenseNumber);
        Task<Owner> GetOwnerByVINAsync(string VIN_number);
        Task SetOwnerAsync(Person owner);
        Task SetDriverAsync(Person driver);
        Task DeletePersonAsync(int personId);
        Task<List<Tuple<Driver, int>>> GetDriversAndPoints();
    }
}
