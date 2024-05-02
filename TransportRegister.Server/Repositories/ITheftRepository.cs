using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories
{
    public interface ITheftRepository
    {
        Task<IEnumerable<TheftListItemDto>> GetActiveThefts();
        Task<TheftDetailDto> GetTheftById(int theftId);
        Task<int> CreateTheft(TheftCreateDto theft, string officerId);
        Task ReportTheftDiscovery(int theftId);
    }
}
