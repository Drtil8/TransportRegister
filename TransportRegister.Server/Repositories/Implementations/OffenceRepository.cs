﻿using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.FineDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class OffenceRepository(AppDbContext context) : IOffenceRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<OffenceListItemDto>> GetUnresolvedOfficialsOffencesAsync(string officialId)
        {
            var offences = await _context.Offences
                .Where(of => of.OfficialId == officialId && !of.IsApproved && of.IsValid)
                .Include(of => of.OffenceOnVehicle)
                .Include(of => of.OffenceOnVehicle.LicensePlates)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    //OffenceType = of.OffenceType, // TODO
                    ReportedOn = of.ReportedOn,
                    //VIN = of.OffenceOnVehicle.VIN,
                    LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                    IsPaid = !of.Fine.IsActive,
                    Amount = of.Fine.Amount
                }).ToListAsync();

            return offences;
        }

        public async Task<IEnumerable<OffenceListItemDto>> GetPersonsOffencesAsync(int personId)
        {
            // TODO
            var offences = await _context.Offences
                .Where(of => of.PersonId == personId && of.IsApproved && of.IsValid)
                .Include(of => of.OffenceOnVehicle)
                .Include(of => of.OffenceOnVehicle.LicensePlates)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    //OffenceType = of.OffenceType, // TODO
                    ReportedOn = of.ReportedOn,
                    //VIN = of.OffenceOnVehicle.VIN,
                    LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                    IsPaid = !of.Fine.IsActive,
                    Amount = of.Fine.Amount
                }).ToListAsync();

            return offences;
        }

        public async Task<IEnumerable<OffenceListItemDto>> GetVehiclesOffencesAsync(int vehicleId)
        {
            // TODO 
            var offences = await _context.Offences
                .Where(of => of.VehicleId == vehicleId && of.IsApproved && of.IsValid)
                .Include(of => of.OffenceOnVehicle)
                .Include(of => of.OffenceOnVehicle.LicensePlates)
                .Select(of => new OffenceListItemDto
                {
                    OffenceId = of.OffenceId,
                    //OffenceType = of.OffenceType, // TODO
                    ReportedOn = of.ReportedOn,
                    //VIN = of.OffenceOnVehicle.VIN,
                    LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault(),
                    IsPaid = !of.Fine.IsActive,
                    Amount = of.Fine.Amount
                }).ToListAsync();

            return offences;
        }

        public async Task<OffenceDetailDto> GetOffenceByIdAsync(int offenceId)
        {
            var offenceDto = await _context.Offences
                .Where(of => of.OffenceId == offenceId)
                .Include(of => of.OffenceOnVehicle)
                .Include(of => of.OffenceOnVehicle.LicensePlates)
                .Include(of => of.Fine)
                .Include(of => of.OffenceType)
                .Select(of => new OffenceDetailDto
                {
                    OffenceId = of.OffenceId,
                    ReportedOn = of.ReportedOn,
                    //Address = of.Address, // TODO
                    Type = of.OffenceType.Name,
                    IsValid = of.IsValid,
                    IsApproved = of.IsApproved,
                    Description = of.Description,
                    Vehicle = of.VehicleId == null ? 
                    new VehicleListItemDto // TODO -> doesnt have to be specified, can be null
                    {
                        Id = of.OffenceOnVehicle.VehicleId,
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault()
                    } : null,
                    PenaltyPoints = of.PenaltyPoints
                    // TODO -> person dto
                }).FirstOrDefaultAsync();

            var fine = await _context.Fines
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

            if (offenceDto == null)
            {
                return null;
            }

            return offenceDto;
        }

        public async Task<IEnumerable<OffenceTypeDto>> GetOffenceTypesAsync()
        {
            var offenceTypes = await _context.OffenceTypes
                .Select(ot => new OffenceTypeDto
                {
                    Id = ot.OffenceTypeId,
                    Name = ot.Name,
                }).ToListAsync();

            return offenceTypes;
        }

        public async Task<bool> AssignOffenceToOfficialAsync(Offence offence) //(int offenceId)
        {
            var official = (await _context.Officials.Where(of => of.IsValid && of.IsActive)
                .Select(o => new 
                {
                    Official = o,
                    OffencesCount = o.ProcessedOffences.Count(off => !off.IsApproved && off.IsValid)
                }).OrderBy(x => x.OffencesCount).FirstOrDefaultAsync())?.Official;

            if (official == null)
            {
                // TODO -> assign to some default official or to official who is on vacation
                return false;
            }

            offence.OfficialId = official.Id;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Offence> ReportOffenceAsync(OffenceCreateDto offenceDto, User activeUser)
        {
            // TODO -> implement
            var offence = new Offence
            {
                OffenceTypeId = offenceDto.OffenceTypeId,
                ReportedOn = DateTime.Now,
                Description = offenceDto.Description,
                PenaltyPoints = offenceDto.PenaltyPoints,
                //Address = new Address // TODO
                //{
                //    City = offenceDto.Address.City,
                //    Street = offenceDto.Address.Street,
                //    State = offenceDto.Address.State,
                //    Country = offenceDto.Address.Country,
                //    HouseNumber = offenceDto.Address.HouseNumber,
                //    PostalCode = offenceDto.Address.PostalCode
                //},
                IsApproved = false,
                IsValid = true,
                VehicleId = offenceDto.VehicleId != 0 ? offenceDto.VehicleId : null,
                PersonId = offenceDto.PersonId,
                OfficerId = activeUser.Id
            };

            if (offenceDto.FineAmount != 0) {
                offence.Fine = new Fine
                {
                    Amount = offenceDto.FineAmount,
                    IsActive = !offenceDto.FinePaid,
                    PaidOn = offenceDto.FinePaid ? DateOnly.FromDateTime(offence.ReportedOn) : DateOnly.Parse("0001-01-01"),
                    DueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7))
            };
            }

            _context.Offences.Add(offence);
            var res = await _context.SaveChangesAsync();
            if (res == 0)
            {
                return null;
            }

            return offence;
        }

        public async Task<bool> ApproveOffenceAsync(int offenceId, OffenceCreateDto offenceDto)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            offence.Description = offenceDto.Description;
            offence.PenaltyPoints = offenceDto.PenaltyPoints;
            offence.Fine.Amount = offenceDto.FineAmount;
            //offence.Fine.IsActive = !offenceDto.FinePaid; // TODO
            //offence.Type = offenceDto.Type; // TODO
            offence.IsApproved = true;
            offence.IsValid = true;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeclineOffenceAsync(int offenceId)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            offence.IsApproved = false;
            offence.IsValid = false;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> EditOffenceAsync(int offenceId, OffenceDetailDto offenceDto)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return -1;
            }

            offence.Description = offenceDto.Description;
            //offence.PenaltyPoints = offenceDto.PenaltyPoints; // TODO
            offence.Fine.Amount = offenceDto.Fine.Amount;
            //offence.Fine.IsActive = !offenceDto.Fine.IsPaid; // TODO
            return -1;
        }

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
