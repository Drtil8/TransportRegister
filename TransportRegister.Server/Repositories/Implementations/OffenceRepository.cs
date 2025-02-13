﻿using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.FineDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    /// <summary>
    /// Repository for handling offences. Implements IOffenceRepository.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class OffenceRepository : IOffenceRepository
    {
        private readonly AppDbContext _context;

        public OffenceRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Static method to get address as string.
        /// </summary>
        /// <param name="address"> Address to be convertred. </param>
        /// <returns> Returns address as string. </returns>
        public static string GetAddresAsString(Address address)
        {
            var finalString = "";
            bool isFirst = true;

            if (address == null)
            {
                return finalString;
            }

            if (address.Country != null && address.Country != "")
            {
                isFirst = false;
                finalString += address.Country;
            }

            if (address.State != null && address.State != "")
            {
                if (!isFirst)
                {
                    finalString += ", ";
                }
                else
                {
                    isFirst = false;
                }
                finalString += address.State;
            }

            if (address.City != null && address.City != "")
            {
                if (!isFirst)
                {
                    finalString += ", ";
                }
                else
                {
                    isFirst = false;
                }
                finalString += address.City;
            }

            if (address.Street != null && address.Street != "")
            {
                if (!isFirst)
                {
                    finalString += ", ";
                }
                else
                {
                    isFirst = false;
                }
                finalString += address.Street;
            }

            if (address.HouseNumber != 0)
            {
                if (!isFirst)
                {
                    finalString += ", ";
                }
                else
                {
                    isFirst = false;
                }
                finalString += address.HouseNumber.ToString();
            }

            if (address.PostalCode != 0)
            {
                if (!isFirst)
                {
                    finalString += ", ";
                }
                else
                {
                    isFirst = false;
                }
                finalString += address.PostalCode.ToString();
            }

            return finalString;
        }

        ////////////////// QUERIES //////////////////

        /// <summary>
        /// Query all offences from database.
        /// </summary>
        /// <returns> Returns query of DTOs. </returns>
        public IQueryable<OffenceListItemDto> QueryAllOffences()
        {
            return _context.Offences
                .AsNoTracking()
                .Include(of => of.OffenceType)
                .Include(of => of.CommitedBy)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    OffenceType = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    PenaltyPoints = of.PenaltyPoints,
                    Person = new PersonSimpleDto
                    {
                        PersonId = of.PersonId,
                        FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                        BirthNumber = of.CommitedBy.BirthNumber
                    },
                    Vehicle = of.VehicleId == null ? null :
                    new VehicleSimpleDto
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                    }
                });
        }

        /// <summary>
        /// Query unresolved offences from database.
        /// </summary>
        /// <returns> Returns query of DTOs. </returns>
        public IQueryable<OffenceListItemDto> QueryUnresolvedOffences()
        {
            return _context.Offences
                .AsNoTracking()
                .Include(of => of.OffenceType)
                .Include(of => of.CommitedBy)
                .Where(of => !of.IsApproved && of.IsValid)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    OffenceType = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    PenaltyPoints = of.PenaltyPoints,
                    Person = new PersonSimpleDto
                    {
                        PersonId = of.PersonId,
                        FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                        BirthNumber = of.CommitedBy.BirthNumber
                    },
                    Vehicle = of.VehicleId == null ? null : 
                    new VehicleSimpleDto
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates
                            .OrderByDescending(lp => lp.ChangedOn)
                            .Select(lp => lp.LicensePlate)
                            .FirstOrDefault(),
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                    }
                });
        }

        ////////////////// FILTER //////////////////

        /// <summary>
        /// Applies filters to query of offences.
        /// </summary>
        /// <param name="query"> Query we apply filters to. </param>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns query with applied params. </returns>
        /// <author> David Drtil </author>
        public IQueryable<OffenceListItemDto> ApplyFilterQueryOffences(IQueryable<OffenceListItemDto> query, DtParamsDto dtParams)
        {
            foreach (var filter in dtParams.Filters)
            {
                query = filter.PropertyName switch
                {
                    nameof(OffenceListItemDto.OffenceId) =>
                        query.Where(o => o.OffenceId.ToString().StartsWith(filter.Value)),
                    nameof(OffenceListItemDto.OffenceType) =>
                        query.Where(o => o.OffenceType.StartsWith(filter.Value)),
                    nameof(OffenceListItemDto.ReportedOn) =>
                        query.Where(o => o.ReportedOn >= DtParamsDto
                            .ParseClientDate(filter.Value, DateTime.MaxValue)),
                    nameof(OffenceListItemDto.PenaltyPoints) =>
                        query.Where(o => o.PenaltyPoints.ToString().StartsWith(filter.Value)),
                    "Person.FullName" =>
                        query.Where(o => o.Person.FullName.StartsWith(filter.Value)),
                    "Person.BirthNumber" =>
                        query.Where(o => o.Person.BirthNumber.StartsWith(filter.Value)),
                    "Vehicle.LicensePlate" =>
                        query.Where(o => o.Vehicle.LicensePlate.StartsWith(filter.Value)),
                    _ => query
                };
            }

            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                return query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(o => o.OffenceId);
            }
            else
            {
                return query.OrderByDescending(o => o.OffenceId);
            }
        }

        ////////////////// GETTERS //////////////////

        /// <summary>
        /// Gets offence by id from database. Includes all related entities.
        /// </summary>
        /// <param name="offenceId"> If of offence. </param>
        /// <param name="user"> User logged in at the moment. </param>
        /// <returns> Returns offence as DTO. </returns>
        public async Task<OffenceDetailDto> GetOffenceByIdAsync(int offenceId, User user)
        {
            var offenceDto = await _context.Offences
                .AsNoTracking()
                .Where(of => of.OffenceId == offenceId)
                .Include(of => of.OffenceOnVehicle)
                .Include(of => of.OffenceOnVehicle.LicensePlates)
                .Include(of => of.Fine)
                .Include(of => of.OffenceType)
                .Select(of => new OffenceDetailDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    Address = OffenceRepository.GetAddresAsString(of.Address),
                    Type = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    Description = of.Description,
                    Vehicle = of.VehicleId == null ?
                    null :
                    new VehicleSimpleDto
                    {
                        VehicleId = of.OffenceOnVehicle.VehicleId,
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault()
                    },
                    PenaltyPoints = of.PenaltyPoints,
                    IsResponsibleOfficial = of.OfficialId == user.Id,
                    Person = new PersonSimpleDto
                    {
                        PersonId = of.PersonId,
                        FullName = of.CommitedBy.FirstName + " " + of.CommitedBy.LastName,
                        BirthNumber = of.CommitedBy.BirthNumber
                    },
                    Officer = new UserSimpleDto
                    {
                        Id = of.OfficerId,
                        FullName = of.ReportedByOfficer.FirstName + " " + of.ReportedByOfficer.LastName
                    },
                    Official = of.OfficialId == null ? null : new UserSimpleDto
                    {
                        Id = of.OfficialId,
                        FullName = of.ProcessedByOfficial.FirstName + " " + of.ProcessedByOfficial.LastName
                    }
                }).FirstOrDefaultAsync();

            var fine = await _context.Fines
                .AsNoTracking()
                .Where(f => f.OffenceId == offenceId)
                .Select(f => new FineDetailDto
                {
                    FineId = f.FineId,
                    Amount = f.Amount,
                    PaidOn = f.PaidOn,
                    DueDate = f.DueDate,
                    IsActive = f.IsActive,
                    IsPaid = !f.IsActive
                }).FirstOrDefaultAsync();

            if (fine != null)
            {
                offenceDto.Fine = fine;
            }

            var photos = await _context.OffencePhotos
                .Where(op => op.OffenceId == offenceId)
                .Select(op => op.Image)
                .ToListAsync();

            if (photos != null)
            {
                foreach (var photo in photos)
                {
                    offenceDto.OffencePhotos64.Add(Convert.ToBase64String(photo));
                }
            }

            if (offenceDto == null)
            {
                return null;
            }

            return offenceDto;
        }

        /// <summary>
        /// Gets all offence types from database. Used for dropdowns.
        /// </summary>
        /// <returns> Returns list of offence types as DTOs. </returns>
        public async Task<IEnumerable<OffenceTypeDto>> GetOffenceTypesAsync()
        {
            var offenceTypes = await _context.OffenceTypes
                .AsNoTracking()
                .Select(ot => new OffenceTypeDto
                {
                    Id = ot.OffenceTypeId,
                    Name = ot.Name,
                    PenaltyPoints = ot.PenaltyPoints,
                    FineAmount = ot.FineAmount
                }).ToListAsync();

            return offenceTypes;
        }

        ////////////////// ACTIONS //////////////////
      
        public Offence CreateOffence(OffenceCreateDto offenceDto, User activeUser)
        {
            var offence = new Offence
            {
                OffenceTypeId = offenceDto.OffenceTypeId,
                ReportedOn = DateTime.Now,
                Description = offenceDto.Description,
                PenaltyPoints = offenceDto.PenaltyPoints,
                Address = new Address
                {
                    Street = offenceDto.Address.Street,
                    City = offenceDto.Address.City,
                    State = offenceDto.Address.State,
                    HouseNumber = offenceDto.Address.HouseNumber,
                    PostalCode = offenceDto.Address.PostalCode,
                    Country = offenceDto.Address.Country
                },
                IsApproved = false,
                IsValid = true,
                VehicleId = offenceDto.VehicleId != 0 ? offenceDto.VehicleId : null,
                PersonId = offenceDto.PersonId,
                OfficerId = activeUser.Id
            };

            return offence;
        }

        /// <summary>
        /// Creates new offence to the database. Sets all properties from DTO.
        /// </summary>
        /// <param name="offenceDto"> DTO with info about new offence. </param>
        /// <param name="activeUser"> User which is logged at the moment. </param>
        /// <returns> Returns new entity. </returns>
        public async Task<Offence> ReportOffenceAsync(OffenceCreateDto offenceDto, User activeUser)
        {
            var offence = CreateOffence(offenceDto, activeUser);

            if (offenceDto.FineAmount != 0)
            {
                offence.Fine = new Fine
                {
                    Amount = offenceDto.FineAmount,
                    IsActive = true,
                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
                };

                if (offenceDto.FinePaid)
                {
                    offence.Fine.PaidOn = DateOnly.FromDateTime(offence.ReportedOn);
                    offence.Fine.IsActive = false;
                    offence.IsApproved = true; // Automatically processing offence when it was dealt with on place
                    if (offence.PenaltyPoints > 0)
                    {
                        await AssignPoints(offence.PersonId, offence.PenaltyPoints);
                    }
                }
            }
            else // If there is no fine, offence is automatically approved
            {
                offence.IsApproved = true;
            }


            _context.Offences.Add(offence);
            var res = await _context.SaveChangesAsync();
            if (res == 0)
            {
                return null;
            }

            if(offenceDto.Photos != null)
            {
                foreach (var photo in offenceDto.Photos)
                {
                    if(!string.IsNullOrEmpty(photo))
                    {
                        var image = Convert.FromBase64String(photo);
                        var offencePhoto = new OffencePhoto
                        {
                            Image = image,
                            OffenceId = offence.OffenceId
                        };
                        _context.OffencePhotos.Add(offencePhoto);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return offence;
        }

        /// <summary>
        /// Assigns points to driver. If driver has more than 12 points, his license is suspended.
        /// </summary>
        /// <param name="driverId"> Id of a driver we assign points to. </param>
        /// <param name="points"> Number of points to be assigned. </param>
        /// <returns></returns>
        public async Task<bool> AssignPoints(int driverId, int points)
        {
            var driver = await _context.Drivers.FindAsync(driverId);
            if (driver == null)
            {
                return false;
            }

            driver.BadPoints += points;
            driver.LastCrimeCommited = DateTime.Now;
            if (driver.BadPoints >= 12)
            {
                driver.BadPoints = 12;
                driver.HasSuspendedLicense = true;
                driver.DrivingSuspendedUntil = DateTime.Now.AddYears(1);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Approves offence. Sets it as valid and approved.
        /// </summary>
        /// <param name="offenceId"> If of offence. </param>
        /// <param name="officialId"> Id of official who did this operation. </param>
        /// <param name="offenceDto"> DTO which will recieve new info. </param>
        /// <returns></returns>
        public async Task<bool> ApproveOffenceAsync(int offenceId, string officialId, OffenceDetailDto offenceDto)
        {
            var offence = await _context.Offences.Include(of => of.Fine).Where(of => of.OffenceId == offenceId).FirstOrDefaultAsync();
            if (offence == null)
            {
                return false;
            }

            if(offence.Fine != null)
            {
                if(offence.Fine.IsActive)
                {
                    offence.Fine.IsActive = false;
                    offence.Fine.PaidOn = DateOnly.FromDateTime(DateTime.Now);
                }
            }

            offence.IsApproved = true;
            offence.IsValid = true;
            offence.OfficialId = officialId;

            if(await _context.SaveChangesAsync() > 0)
            {
                offenceDto.IsApproved = offence.IsApproved;
                offenceDto.IsValid = offence.IsValid;
                offenceDto.Official = new UserSimpleDto
                {
                    Id = officialId,
                    FullName = offence.ProcessedByOfficial.FirstName + " " + offence.ProcessedByOfficial.LastName
                };
                if(offenceDto.Fine != null)
                {
                    offenceDto.Fine.IsPaid = !offence.Fine.IsActive;
                    offenceDto.Fine.IsActive = offence.Fine.IsActive;
                    offenceDto.Fine.PaidOn = offence.Fine.PaidOn;
                }
            }
            else
            {
                return false;
            }
            
            if(offence.PenaltyPoints > 0)
            {
                var res = await AssignPoints(offence.PersonId, offence.PenaltyPoints);
            }

            return true;
        }

        /// <summary>
        /// Declines offence. Sets it as not valid and not approved.
        /// </summary>
        /// <param name="offenceId"> If of offence. </param>
        /// <param name="officialId"> Id of official who did this operation. </param>
        /// <returns></returns>
        public async Task<bool> DeclineOffenceAsync(int offenceId, string officialId)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            offence.IsApproved = false;
            offence.IsValid = false;
            offence.OfficialId = officialId;

            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="offenceId"> If of offence. </param>
        /// <param name="offenceDto"></param>
        /// <returns></returns>
        public async Task<int> EditOffenceAsync(int offenceId, OffenceDetailDto offenceDto)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return -1;
            }

            offence.Description = offenceDto.Description;
            //offence.PenaltyPoints = offenceDto.PenaltyPoints;
            offence.Fine.Amount = offenceDto.Fine.Amount;
            //offence.Fine.IsActive = !offenceDto.Fine.IsPaid;
            return -1;
        }

        /// <summary>
        /// Deltes offence from database. Not used.
        /// </summary>
        /// <param name="offenceId"> If of offence. </param>
        /// <returns> Returns true if operation was successful. </returns>
        public async Task<bool> DeleteOffenceAsync(int offenceId)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            _context.Offences.Remove(offence);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
