using Microsoft.EntityFrameworkCore;
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
                .Select(of => new OffenceDetailDto
                {
                    OffenceId = of.OffenceId,
                    //OffenceType = of.OffenceType, // TODO
                    ReportedOn = of.ReportedOn,
                    Description = of.Description,
                    IsApproved = of.IsApproved,
                    IsValid = of.IsValid,
                    Vehicle = new VehicleListItemDto // TODO
                    {
                        Id = of.OffenceOnVehicle.VehicleId,
                        Manufacturer = of.OffenceOnVehicle.Manufacturer,
                        Model = of.OffenceOnVehicle.Model,
                        VIN = of.OffenceOnVehicle.VIN,
                        LicensePlate = of.OffenceOnVehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault()
                    },
                    Fine = new FineDetailDto
                    {
                        FineId = of.Fine.FineId,
                        Amount = of.Fine.Amount,
                        PaidOn = of.Fine.PaidOn,
                        IsActive = of.Fine.IsActive,
                        IsPaid = !of.Fine.IsActive
                    }
                }).FirstOrDefaultAsync();

            if (offenceDto == null)
            {
                return null;
            }

            return offenceDto;
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
                //OffenceType = offenceDto.OffenceType, // TODO
                ReportedOn = DateTime.Now,
                Description = offenceDto.Description,
                PenaltyPoints = offenceDto.PenaltyPoints,
                Address = new Address
                {
                    City = offenceDto.Address.City,
                    Street = offenceDto.Address.Street,
                    State = offenceDto.Address.State,
                    Country = offenceDto.Address.Country,
                    HouseNumber = offenceDto.Address.HouseNumber,
                    PostalCode = offenceDto.Address.PostalCode
                },
                IsApproved = false,
                IsValid = true,
                VehicleId = offenceDto.VehicleId,
                PersonId = offenceDto.PersonId,
                OfficerId = activeUser.Id
            };

            if (offenceDto.FineAmount != 0) { // TODO -> mby create fine first and then assign?
                offence.Fine = new Fine
                {
                    Amount = offenceDto.FineAmount,
                    IsActive = !offenceDto.FinePaid
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
