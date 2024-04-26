using TransportRegister.Server.DTOs.TheftDTOs;

namespace TransportRegister.Server.Repositories
{
    public interface ITheftRepository
    {
        Task<IEnumerable<TheftListItemDto>> GetActiveThefts();
        Task<TheftDetailDto> GetTheftById(int theftId);
        Task<int> CreateTheft(TheftDetailDto theft);
        Task ReportTheftDiscovery(int theftId);
    }
}
