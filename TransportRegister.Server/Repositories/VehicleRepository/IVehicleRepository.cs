using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.VehicleRepository
{
    public interface IVehicleRepository
    {
        Task<List<string>> GetVehicleTypesAsync();
        Task<List<LicensePlateHistoryDto>> GetLicensePlateHistoryAsync(int vehicleId);
        Task<Vehicle> GetVehicleByIdAsync(int vehicleId);
        Task DeleteVehicleAsync(int vehicleId);
        Task SaveVehicleAsync(Vehicle vehicle);
        IQueryable<VehicleListItemDto> QueryVehicleSearch(DtParamsDto dtParams);
    }
}
