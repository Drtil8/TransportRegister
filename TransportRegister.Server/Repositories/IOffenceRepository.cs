using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories
{
    public interface IOffenceRepository
    {
        IQueryable<OffenceListItemDto> QueryAllOffences();
        IQueryable<OffenceListItemDto> QueryUnresolvedOffences();
        IQueryable<OffenceListItemDto> ApplyFilterQueryOffences(IQueryable<OffenceListItemDto> query, DtParamsDto dtParams);
        Task<OffenceDetailDto> GetOffenceByIdAsync(int offenceId, User user);
        Task<Offence> ReportOffenceAsync(OffenceCreateDto offenceDto, User activeUser);
        Task<bool> ApproveOffenceAsync(int offenceId, string officialId, OffenceDetailDto offenceDto);
        Task<bool> DeclineOffenceAsync(int offenceId, string officialId);
        Task<int> EditOffenceAsync(int offenceId, OffenceDetailDto offenceDto);
        Task<bool> DeleteOffenceAsync(int offenceId);
        Task<IEnumerable<OffenceTypeDto>> GetOffenceTypesAsync();
    }
}
