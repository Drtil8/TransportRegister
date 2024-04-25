using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories
{
    public interface IOffenceRepository
    {
        Task<IEnumerable<OffenceListItemDto>> GetUnresolvedOfficialsOffences(string officialId);
        Task<IEnumerable<OffenceListItemDto>> GetPersonsOffences(int personId);
        Task<IEnumerable<OffenceListItemDto>> GetVehiclesOffences(int vehicleId);
        Task<OffenceDetailDto> GetOffenceById(int offenceId);
        Task<bool> AssignOffenceToOfficial(Offence offence); //(int offenceId);
        Task<int> ReportOffence(OffenceCreateDto offenceDto);
        Task<int> ResolveOffence(int offenceId, bool action);
        Task<int> EditOffence(OffenceDetailDto offenceDto); // TODO -> change dto?
        Task<bool> DeleteOffence(int offenceId);
        // TODO -> define methods
    }
}
