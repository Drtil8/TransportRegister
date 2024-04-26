using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories
{
    public interface IOffenceRepository
    {
        Task<IEnumerable<OffenceListItemDto>> GetUnresolvedOfficialsOffencesAsync(string officialId);
        Task<IEnumerable<OffenceListItemDto>> GetPersonsOffencesAsync(int personId);
        Task<IEnumerable<OffenceListItemDto>> GetVehiclesOffencesAsync(int vehicleId);
        Task<OffenceDetailDto> GetOffenceByIdAsync(int offenceId);
        Task<bool> AssignOffenceToOfficialAsync(Offence offence); //(int offenceId);
        Task<Offence> ReportOffenceAsync(OffenceCreateDto offenceDto);
        Task<bool> ResolveOffenceAsync(int offenceId, bool action);
        Task<int> EditOffenceAsync(OffenceDetailDto offenceDto); // TODO -> change dto?
        Task<bool> DeleteOffenceAsync(int offenceId);
        // TODO -> define methods
    }
}
