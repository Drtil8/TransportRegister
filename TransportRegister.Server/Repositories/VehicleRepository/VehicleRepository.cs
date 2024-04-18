using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.VehicleRepository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GetVehicleTypesAsync()
        {
            List<string> types = _context.Model.GetEntityTypes()
                .Where(t => t.ClrType.IsSubclassOf(typeof(Vehicle)))
                .Select(t => t.ClrType.Name)
                .Distinct()
                .ToList();

            return await Task.FromResult(types);
        }

        public async Task<List<LicensePlateHistoryDto>> GetLicensePlateHistoryAsync(int vehicleId)
        {
            return await _context.LicensePlates
                .Where(lph => lph.VehicleId == vehicleId)
                .Select(lph => new LicensePlateHistoryDto
                {
                    LicensePlateHistoryId = lph.LicensePlateHistoryId,
                    LicensePlate = lph.LicensePlate,
                    ChangedOn = lph.ChangedOn,
                    VehicleId = lph.VehicleId
                })
                .ToListAsync();
        }
        
        public async Task<Vehicle> GetVehicleAsync(int vehicleId)
        {
            return await _context.Vehicles
                .Include(v => v.LicensePlates)
                .Include(v => v.Offences)
                .Include(v => v.Thefts)
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task SaveVehicleAsync(Vehicle vehicle)
        {
            if (vehicle.VehicleId == 0)
            {
                _context.Vehicles.Add(vehicle);
            }
            else
            {
                _context.Vehicles.Update(vehicle);
            }
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteVehicleAsync(int vehicleId)
        {
            Vehicle vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
