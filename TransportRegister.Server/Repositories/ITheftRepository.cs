using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories
{
    public interface ITheftRepository
    {
        IQueryable<TheftListItemDto> QueryAllThefts();
        IQueryable<TheftListItemDto> QueryActiveThefts();
        IQueryable<TheftListItemDto> ApplyFilterQueryThefts(IQueryable<TheftListItemDto> query, DtParamsDto dtParams);
        Task<IEnumerable<TheftListItemDto>> GetActiveThefts();
        Task<TheftDetailDto> GetTheftById(int theftId);
        Task<int> CreateTheft(TheftCreateDto theft, string officerId);
        Task ReportTheftDiscovery(int theftId);
    }
}
