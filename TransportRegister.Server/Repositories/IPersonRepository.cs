using TransportRegister.Server.Models;
namespace TransportRegister.Server.Repositories
{
    public interface IPersonRepository
    {

        Task<Person> GetPersonByIdAsync(int personId);
        Task<Driver> GetDriverAsync(string licenseNumber);
        Task<Person> GetOwnerByVINAsync(string VIN_number);
        Task SetOwnerAsync(Person owner);
        Task SetDriverAsync(Person driver);
        Task DeletePersonAsync(int personId);
        Task SavePersonAsync(Person person);
        Task<List<Tuple<Driver, int>>> GetDriversAndPoints();
        Task<List<Theft>> GetPersonReportedTheftsByIdAsync(int personId);
        Task<List<Offence>> GetPersonCommitedOffencesByIdAsync(int personId);
    }
}
