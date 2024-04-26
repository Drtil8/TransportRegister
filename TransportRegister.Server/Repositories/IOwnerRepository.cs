using TransportRegister.Server.Models;
namespace TransportRegister.Server.Repositories

{
    public interface IOwnerRepository
    {
        Task<Owner> GetOwnerByVINAsync(string VIN_number);
        Task SaveOwnerAsync(Owner owner);
        Task DeleteOwnerAsync(int ownerId);
        Task<Owner> GetOwnerByIdAsync(int ownerId);

    }
}
