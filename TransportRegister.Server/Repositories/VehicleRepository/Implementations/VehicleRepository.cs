using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.VehicleRepository.Implementations
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

        public async Task<Vehicle> GetVehicleByIdAsync(int vehicleId)
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

        private static IQueryable<VehicleListItemDto> ApplySortingVehicleSearch(
            IQueryable<VehicleListItemDto> query, DtParamsDto dtParams)
        {
            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                return query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(v => v.VehicleId);
            }
            else
            {
                return query.OrderByDescending(v => v.VehicleId);
            }
        }

        private static IQueryable<VehicleListItemDto> ApplyFilterVehicleSearch(
            IQueryable<VehicleListItemDto> query, DtParamsDto dtParams)
        {
            foreach (var filter in dtParams.Filters)
            {
                // todo string properties can be filtered by Contains or StartsWith
                query = filter.PropertyName switch
                {
                    nameof(VehicleListItemDto.VehicleId) =>
                        query.Where(v => v.VehicleId.ToString().StartsWith(filter.Value)), // numeric property
                    nameof(VehicleListItemDto.VIN) =>
                        query.Where(v => v.VIN.StartsWith(filter.Value)),                  // string property
                    nameof(VehicleListItemDto.LicensePlate) =>
                        query.Where(v => v.LicensePlate.StartsWith(filter.Value)),
                    nameof(VehicleListItemDto.VehicleType) =>
                        query.Where(v => v.VehicleType.StartsWith(filter.Value)),
                    nameof(VehicleListItemDto.Manufacturer) =>
                        query.Where(v => v.Manufacturer.StartsWith(filter.Value)),
                    nameof(VehicleListItemDto.Model) =>
                        query.Where(v => v.Model.StartsWith(filter.Value)),
                    nameof(VehicleListItemDto.Color) =>
                        query.Where(v => v.Color.StartsWith(filter.Value)),
                    nameof(VehicleListItemDto.ManufacturedYear) =>
                        query.Where(v => v.ManufacturedYear.ToString().StartsWith(filter.Value)),
                    nameof(VehicleListItemDto.OwnerFullName) =>
                        query.Where(v => v.OwnerFullName.StartsWith(filter.Value)),
                    _ => query      // Default case - do not apply any filter
                };
            }
            return query;
        }

        public IQueryable<VehicleListItemDto> QueryVehicleSearch(DtParamsDto dtParams)
        {
            //VehicleDetailDto vehicleDto = VehicleDtoTransformer.TransformToDto(vehicle);  // todo check this
            var query = _context.Vehicles
                .AsNoTracking()
                .Include(v => v.LicensePlates)
                .Include(v => v.Owner)
                .Select(v =>
                    new VehicleListItemDto
                    {
                        VehicleId = v.VehicleId,
                        VIN = v.VIN,
                        VehicleType = v.GetType().Name,     // todo probably not right
                        LicensePlate = v.LicensePlates
                            .OrderByDescending(lp => lp.ChangedOn)
                            .Select(lp => lp.LicensePlate)
                            .FirstOrDefault(),
                        Manufacturer = v.Manufacturer,
                        Model = v.Model,
                        Color = v.Color,
                        ManufacturedYear = v.ManufacturedYear,
                        OwnerId = v.OwnerId,
                        OwnerFullName = v.Owner.FirstName + " " + v.Owner.LastName,
                    }
                );
            query = ApplyFilterVehicleSearch(query, dtParams);
            query = ApplySortingVehicleSearch(query, dtParams);
            return query;
        }
    }
}
