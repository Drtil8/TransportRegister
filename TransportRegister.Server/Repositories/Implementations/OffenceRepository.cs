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
            // need to get official with least offences and wchich is not on vacation
            //var offence = await _context.Offences.FindAsync(offenceId); // one less query

            var official = await _context.Officials
                .Where(o => o.IsActive && o.IsValid)
                .OrderBy(o => o.ProcessedOffences.Count)
                .Where(o => o.ProcessedOffences.All(po => !po.IsApproved && po.IsValid))
                .FirstOrDefaultAsync();

            if (official == null)
            {
                return false;
            }

            offence.OfficialId = official.Id;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Offence> ReportOffenceAsync(OffenceCreateDto offenceDto)
        {
            // TODO -> implement
            return null;
        }

        public async Task<bool> ResolveOffenceAsync(int offenceId, bool action)
        {
            var offence = await _context.Offences.FindAsync(offenceId);
            if (offence == null)
            {
                return false;
            }

            offence.IsApproved = action;

            if (!action) // If offence is denied -> it is also taken as invalid
            {
                offence.IsValid = action;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> EditOffenceAsync(OffenceDetailDto offenceDto)
        {
            // TODO -> implement
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
