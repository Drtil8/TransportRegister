using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations;

public class TheftRepository(AppDbContext context) : ITheftRepository
{
    private readonly AppDbContext _context = context;

    public IQueryable<TheftListItemDto> QueryAllThefts()
    {
        return _context.Thefts
            .AsNoTracking()
            .Include(t => t.StolenVehicle)
            .Include(t => t.StolenVehicle.LicensePlates)
            .Select(t => new TheftListItemDto
            {
                TheftId = t.TheftId,
                Vehicle = new VehicleSimpleDto
                {
                    VehicleId = t.VehicleId,
                    VIN = t.StolenVehicle.VIN,
                    LicensePlate = t.StolenVehicle.LicensePlates
                        .OrderByDescending(lp => lp.ChangedOn)
                        .Select(lp => lp.LicensePlate)
                        .FirstOrDefault(),
                    Manufacturer = t.StolenVehicle.Manufacturer,
                    Model = t.StolenVehicle.Model
                },
                StolenOn = t.StolenOn,
                ReportedOn = t.ReportedOn,
                FoundOn = t.FoundOn,
                ReturnedOn = t.ReturnedOn,
                IsFound = t.FoundOn != null,
                IsReturned = t.ReturnedOn != null,
            });
    }

    public IQueryable<TheftListItemDto> QueryActiveThefts()
    {
        return _context.Thefts
            .AsNoTracking()
            .Include(t => t.StolenVehicle)
            .Include(t => t.StolenVehicle.LicensePlates)
            .Where(t => t.FoundOn == null || t.ReturnedOn == null)  // isFound == false or isReturned == false -> is active theft
            .Select(t => new TheftListItemDto
            {
                TheftId = t.TheftId,
                Vehicle = new VehicleSimpleDto
                {
                    VehicleId = t.VehicleId,
                    VIN = t.StolenVehicle.VIN,
                    LicensePlate = t.StolenVehicle.LicensePlates
                        .OrderByDescending(lp => lp.ChangedOn)
                        .Select(lp => lp.LicensePlate)
                        .FirstOrDefault(),
                    Manufacturer = t.StolenVehicle.Manufacturer,
                    Model = t.StolenVehicle.Model
                },
                StolenOn = t.StolenOn,
                ReportedOn = t.ReportedOn,
                FoundOn = t.FoundOn,
                ReturnedOn = t.ReturnedOn,
                IsFound = t.FoundOn != null,
                IsReturned = t.ReturnedOn != null,
            });
    }

    public IQueryable<TheftListItemDto> ApplyFilterQueryThefts(IQueryable<TheftListItemDto> query, DtParamsDto dtParams)
    {
        //todo

        return query;
    }

    public async Task<IEnumerable<TheftListItemDto>> GetActiveThefts()
    {
        return await _context.Thefts
            .AsNoTracking()
            .Include(t => t.StolenVehicle)
            .Include(t => t.StolenVehicle.LicensePlates)
            .Where(t => t.FoundOn == null || t.ReturnedOn == null)  // isFound == false
            .Select(t => new TheftListItemDto
            {
                TheftId = t.TheftId,
                Vehicle = new VehicleSimpleDto
                {
                    VehicleId = t.VehicleId,
                    VIN = t.StolenVehicle.VIN,
                    LicensePlate = t.StolenVehicle.LicensePlates
                        .OrderByDescending(lp => lp.ChangedOn)
                        .Select(lp => lp.LicensePlate)
                        .FirstOrDefault(),
                    Manufacturer = t.StolenVehicle.Manufacturer,
                    Model = t.StolenVehicle.Model
                },
                StolenOn = t.StolenOn,
                ReportedOn = t.ReportedOn,
                FoundOn = t.FoundOn,
                ReturnedOn = t.ReturnedOn,
                IsFound = t.FoundOn != null,
                IsReturned = t.ReturnedOn != null,
            })
            .ToListAsync();
    }

    public async Task<TheftDetailDto> GetTheftById(int theftId)
    {
        return await _context.Thefts
            .AsNoTracking()
            .Where(t => t.TheftId == theftId)
            .Include(t => t.StolenVehicle)
            .Include(t => t.StolenVehicle.LicensePlates)
            .Include(t => t.StolenVehicle.Owner)
            .Select(t => new TheftDetailDto
            {
                TheftId = t.TheftId,
                Address = OffenceRepository.GetAddresAsString(t.Address),
                StolenOn = t.StolenOn,
                ReportedOn = t.ReportedOn,
                FoundOn = t.FoundOn,
                ReturnedOn = t.ReturnedOn,
                IsFound = t.FoundOn != null,
                IsReturned = t.ReturnedOn != null,
                Description = t.Description,
                VehicleId = t.VehicleId,
                StolenVehicle = new VehicleListItemDto
                {
                    Id = t.StolenVehicle.VehicleId,
                    VIN = t.StolenVehicle.VIN,
                    VehicleType = t.StolenVehicle.GetType().Name,
                    LicensePlate = t.StolenVehicle.LicensePlates
                            .OrderByDescending(lp => lp.ChangedOn)
                            .Select(lp => lp.LicensePlate)
                            .FirstOrDefault(),
                    Manufacturer = t.StolenVehicle.Manufacturer,
                    Model = t.StolenVehicle.Model,
                    Color = t.StolenVehicle.Color,
                    ManufacturedYear = t.StolenVehicle.ManufacturedYear,
                    OwnerId = t.StolenVehicle.OwnerId,
                    OwnerFullName = t.StolenVehicle.Owner.FirstName + " " + t.StolenVehicle.Owner.LastName,
                },
                Official = t.ProcessedByOfficial == null ? null : new UserSimpleDto
                {
                    Id = t.ProcessedByOfficial.Id,
                    FullName = t.ProcessedByOfficial.FirstName + " " + t.ProcessedByOfficial.LastName
                },
                OfficerReported = new UserSimpleDto
                {
                    Id = t.ReportedByOfficer.Id,
                    FullName = t.ReportedByOfficer.FirstName + " " + t.ReportedByOfficer.LastName
                },
                OfficerFound = t.ResolvedByOfficer == null ? null : new UserSimpleDto
                {
                    Id = t.ResolvedByOfficer.Id,
                    FullName = t.ResolvedByOfficer.FirstName + " " + t.ResolvedByOfficer.LastName
                }
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateTheft(TheftCreateDto theftDto, string officerId)
    {
        var newTheft = new Theft
        {
            StolenOn = theftDto.StolenOn,
            Address = new Address
            {
                Street = theftDto.Address.Street,
                City = theftDto.Address.City,
                State = theftDto.Address.State,
                HouseNumber = theftDto.Address.HouseNumber,
                PostalCode = theftDto.Address.PostalCode,
                Country = theftDto.Address.Country
            },
            ReportedOn = DateTime.Now,
            VehicleId = theftDto.VehicleId,
            ReportingPersonId = theftDto.ReportingPersonId,
            Description = theftDto.Description,
            ReportingOfficerId = officerId
        };
        await _context.Thefts.AddAsync(newTheft);
        await _context.SaveChangesAsync();
        return newTheft.TheftId;
    }

    public async Task ReportTheftDiscovery(int theftId, string officerId)
    {
        var theft = await _context.Thefts
            .Where(t => t.TheftId == theftId)
            .FirstOrDefaultAsync()
                ?? throw new ApplicationException("Theft not found");
        theft.FoundOn = DateTime.Now;
        theft.ResolvingOfficerId = officerId;
        await _context.SaveChangesAsync();
    }

    public async Task ReportTheftReturn(int theftId, string officialId)
    {
        var theft = await _context.Thefts
            .Where(t => t.TheftId == theftId)
            .FirstOrDefaultAsync()
                ?? throw new ApplicationException("Theft not found");
        theft.ReturnedOn = DateTime.Now;
        theft.OfficialId = officialId;
        await _context.SaveChangesAsync();
    }
}
