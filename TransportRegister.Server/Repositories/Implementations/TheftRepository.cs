using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations;

/// <summary>
/// Repository for handling thefts. Implements ITheftRepository.
/// </summary>
/// <author> David Drtil </author>
/// <author> Dominik Pop </author>
public class TheftRepository(AppDbContext context) : ITheftRepository
{
    private readonly AppDbContext _context = context;

    ////////////////// QUERIES //////////////////

    /// <summary>
    /// Query all thefts from the database
    /// </summary>
    /// <returns> Retruns query of thefts as DTOs. </returns>
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

    /// <summary>
    /// Query all active thefts from the database.
    /// </summary>
    /// <returns> Retruns query of thefts as DTOs. </returns>
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

    ////////////////// FILTER //////////////////

    private static IQueryable<TheftListItemDto> ApplyDateFilter(
        IQueryable<TheftListItemDto> query, DtParamsDto dtParams, ColumnFilter filter)
    {
        string propertyName = filter.PropertyName;
        DateTime selectedDate = DtParamsDto.ParseClientDate(filter.Value, DateTime.MinValue);
        string filterOption = dtParams.FilterOptions.First(f => f.Id == filter.Id).Option;

        var propertyInfo = typeof(TheftListItemDto).GetProperty(propertyName)
            ?? throw new ArgumentException($"Property '{propertyName}' does not exist on type TheftListItemDto.");

        // Create parameters expressions
        var parameter = Expression.Parameter(typeof(TheftListItemDto), "t");
        var property = Expression.Property(parameter, propertyName);
        if (property.Type.IsGenericType && property.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            // Check if the property is nullable, if so get the value property
            property = Expression.Property(property, "Value");
        }
        var selectedDateConstant = Expression.Constant(selectedDate.Date);
        Expression<Func<TheftListItemDto, bool>> predicate = filterOption switch
        {
            "equals" => Expression.Lambda<Func<TheftListItemDto, bool>>(
                Expression.Equal(
                    Expression.Property(property, "Date"),
                    Expression.Property(selectedDateConstant, "Date")), parameter),
            "greaterThan" => Expression.Lambda<Func<TheftListItemDto, bool>>(
                Expression.GreaterThan(
                    Expression.Property(property, "Date"),
                    Expression.Property(selectedDateConstant, "Date")), parameter),
            "lessThan" => Expression.Lambda<Func<TheftListItemDto, bool>>(
                Expression.LessThan(
                    Expression.Property(property, "Date"),
                    Expression.Property(selectedDateConstant, "Date")), parameter),
            _ => throw new ApplicationException("Unknown filter option"),
        };
        return query.Where(predicate);
    }

    /// <summary>
    /// Apply filters to the query of thefts.
    /// </summary>
    /// <param name="query"> Query we apply filter to. </param>
    /// <param name="dtParams"> Datatable parametres. </param>
    /// <returns> Returns query with applied filter. </returns>
    public IQueryable<TheftListItemDto> ApplyFilterQueryThefts(IQueryable<TheftListItemDto> query, DtParamsDto dtParams)
    {
        foreach (var filter in dtParams.Filters)
        {
            query = filter.PropertyName switch
            {
                nameof(TheftListItemDto.TheftId) =>
                    query.Where(t => t.TheftId.ToString().StartsWith(filter.Value)),
                nameof(TheftListItemDto.StolenOn) =>
                    ApplyDateFilter(query, dtParams, filter),
                nameof(TheftListItemDto.ReportedOn) =>
                    ApplyDateFilter(query, dtParams, filter),
                nameof(TheftListItemDto.FoundOn) =>
                    ApplyDateFilter(query, dtParams, filter),
                "Vehicle.LicensePlate" =>
                    query.Where(t => t.Vehicle.LicensePlate.StartsWith(filter.Value)),
                "Vehicle.Manufacturer" =>
                    query.Where(t => t.Vehicle.Manufacturer.StartsWith(filter.Value)),
                "Vehicle.Model" =>
                    query.Where(t => t.Vehicle.Model.StartsWith(filter.Value)),
                _ => query
            };
        }

        if (dtParams.Sorting.Any())
        {
            Sorting sorting = dtParams.Sorting.First();
            return query.OrderBy($"{sorting.Id} {sorting.Dir}")
                .ThenByDescending(t => t.TheftId);
        }
        else
        {
            return query.OrderByDescending(t => t.TheftId);
        }
    }

    ////////////////// GETTERS //////////////////

    /// <summary>
    /// Get active thefts from the database.
    /// </summary>
    /// <returns> Returns list of thefts as DTOs. </returns>
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

    /// <summary>
    /// Get theft by id from the database.
    /// </summary>
    /// <param name="theftId"> Id of theft. </param>
    /// <returns> Returns theft as DTO for detail. </returns>
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

    ////////////////// ACTIONS //////////////////

    /// <summary>
    /// Create theft in the database.
    /// </summary>
    /// <param name="theftDto"> DTO containing info about new theft. </param>
    /// <param name="officerId"> Id of officer who reported theft. </param>
    /// <returns></returns>
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

    /// <summary>
    /// Sets theft to state where vehicle was found.
    /// </summary>
    /// <param name="theftId"> Theft id. </param>
    /// <param name="officerId"> Id of officer who reported finding vehicle. </param>
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

    /// <summary>
    /// Sets theft to state where vehicle was returned to owner.
    /// </summary>
    /// <param name="theftId"> Theft id.  </param>
    /// <param name="officialId"> Id of official which reported vehicle as returned. </param>
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
