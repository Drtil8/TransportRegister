﻿using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
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
                IsFound = t.FoundOn != null,
            });
    }

    public IQueryable<TheftListItemDto> QueryActiveThefts()
    {
        return _context.Thefts
            .AsNoTracking()
            .Include(t => t.StolenVehicle)
            .Include(t => t.StolenVehicle.LicensePlates)
            .Where(t => t.FoundOn == null)  // isFound == false
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
                IsFound = t.FoundOn != null,
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
            .Where(t => t.FoundOn == null)  // isFound == false
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
                IsFound = t.FoundOn != null,
            })
            .ToListAsync();
    }

    public async Task<TheftDetailDto> GetTheftById(int theftId)
    {
        return await _context.Thefts
            .Include(t => t.StolenVehicle)
            .Include(t => t.StolenVehicle.LicensePlates)
            .Include(t => t.StolenVehicle.Owner)
            .Where(t => t.TheftId == theftId)
            .Select(t => new TheftDetailDto
            {
                TheftId = t.TheftId,
                StolenOn = t.StolenOn,
                ReportedOn = t.ReportedOn,
                FoundOn = t.FoundOn,
                IsFound = t.FoundOn != null,
                Description = t.Description,
                VehicleId = t.VehicleId,
                StolenVehicle = new DTOs.VehicleDTOs.VehicleListItemDto
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
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> CreateTheft(TheftCreateDto theftDto, string officerId)
    {
        var newTheft = new Theft
        {
            StolenOn = theftDto.StolenOn,
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

    public async Task ReportTheftDiscovery(int theftId)
    {
        var theft = await _context.Thefts
            .Where(t => t.TheftId == theftId)
            .FirstOrDefaultAsync()
                ?? throw new ApplicationException("Theft not found");
        theft.FoundOn = DateTime.Now;
        await _context.SaveChangesAsync();
    }
}
