using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;

namespace TransportRegister.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffenceController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IOffenceRepository _offenceRepository;
        private readonly UserManager<User> _userManager;

        public OffenceController(AppDbContext context, IOffenceRepository offenceRepository, UserManager<User> userManager)
        {
            _context = context;
            _offenceRepository = offenceRepository;
            _userManager = userManager;
        }

        ////////////////// GET METHODS //////////////////

        // GET: api/Offence/5/UnresolvedOffences
        [HttpGet("{officialId}/UnresolvedOffences")]
        public async Task<ActionResult<IEnumerable<Offence>>> GetUnresolvedOffences(string officialId)
        {
            var offences = await _offenceRepository.GetUnresolvedOfficialsOffencesAsync(officialId);

            if (offences == null)
            {
                return NotFound("Nenalezeny žádné přestupky.");
            }

            return Ok(offences);
        }

        // GET: api/Offence/5
        // url = api/Offence/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OffenceDetailDto>> GetOffence(int id)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            var offence = await _offenceRepository.GetOffenceByIdAsync(id, activeUser);

            if (offence == null)
            {
                return NotFound("Hledaný přestupek neexistuje.");
            }

            return Ok(offence);
        }

        [HttpGet("GetOffenceTypes")]
        [Authorize(Roles = "Officer")]
        public async Task<ActionResult<IEnumerable<OffenceTypeDto>>> GetOffenceTypes()
        {
            var offenceTypes = await _offenceRepository.GetOffenceTypesAsync();

            if (offenceTypes == null)
            {
                return NotFound("Nenalezeny žádné typy přestupků.");
            }

            return Ok(offenceTypes);
        }

        ////////////////// POST METHODS //////////////////

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // url = api/Offence/ReportOffence
        [HttpPost("ReportOffence")]
        [Authorize(Roles = "Officer")]
        public async Task<ActionResult<int>> ReportOffence(OffenceCreateDto offenceDto)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            var offence = await _offenceRepository.ReportOffenceAsync(offenceDto, activeUser);

            if (offence == null)
            {
                return BadRequest("Přestupek se nepodařilo nahlásit.");
            }

            var assigned = await _offenceRepository.AssignOffenceToOfficialAsync(offence);
            if (!assigned)
            {
                return BadRequest("Přestupek se nepodařilo přiřadit k úředníkovi.");
            }

            return Ok(offence.OffenceId);
        }

        // url = api/Offence/GetVehicleForReport
        [HttpPost("GetVehicleForReport")]
        [Authorize(Roles = "Officer")]
        public async Task<ActionResult<OffenceVehicleDto>> GetVehicleForReport(OffenceVehicleDto vehicleDto)
        {
            Vehicle vehicle;
            if(vehicleDto.LicensePlate != "")
            {
                var licensePlate = await _context.LicensePlates.Where(lp => lp.LicensePlate == vehicleDto.LicensePlate).Include(lp => lp.Vehicle).FirstOrDefaultAsync();
                if (licensePlate == null)
                {
                    return NotFound("* SPZ nenalezena v systému.");
                }
                vehicle = licensePlate.Vehicle;
            }
            else
            {
                vehicle = await _context.Vehicles.Where(v => v.VIN == vehicleDto.VIN).Include(v => v.LicensePlates).FirstOrDefaultAsync();
            }

            if (vehicle == null)
            {
                return NotFound("* Vozidlo nebylo nalezeno.");
            }

            vehicleDto.VehicleId = vehicle.VehicleId;
            vehicleDto.LicensePlate = vehicle.LicensePlates.OrderByDescending(lp => lp.ChangedOn).Select(lp => lp.LicensePlate).FirstOrDefault();
            vehicleDto.Manufacturer = vehicle.Manufacturer;
            vehicleDto.Model = vehicle.Model;
            vehicleDto.VIN = vehicle.VIN;

            return Ok(vehicleDto);
        }

        ////////////////// PUT METHODS //////////////////

        // PUT: api/Offence/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffence(int id, OffenceCreateDto offenceDto)
        {
            var offence = await _context.Offences.Where(of => of.OffenceId == id).Include(of => of.Fine).FirstOrDefaultAsync();
            if (offence == null)
            {
                return NotFound();
            }

            offence.PenaltyPoints = offenceDto.PenaltyPoints;
            if(offence.Fine != null)
            {
                offence.Fine.Amount = offenceDto.FineAmount;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}/Approve")]
        [Authorize(Roles = "Official")]
        public async Task<ActionResult<OffenceDetailDto>> ApproveOffence(int id, OffenceDetailDto offenceDto)
        {
            var result = await _offenceRepository.ApproveOffenceAsync(id, offenceDto);
            if (!result)
            {
                return BadRequest("Přestupek se nepodařilo schválit.");
            }

            return Ok(offenceDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offenceId"></param>
        /// <returns></returns>
        /// url = api/Offence/5/Decline
        [HttpPut("{offenceId}/Decline")]
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> DeclineOffence(int offenceId)
        {
            var result = await _offenceRepository.DeclineOffenceAsync(offenceId);
            if (!result)
            {
                return BadRequest("Přestupek se nepodařilo zamítnout.");
            }

            return Ok("Přestupek byl úspěšně zamítnut.");
        }

        [HttpPut("{offenceId}/PayFine")]
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> PayFine(int offenceId)
        {
            var offence = await _context.Offences.Where(of => of.OffenceId == offenceId).Include(of => of.Fine).FirstOrDefaultAsync();
            if (offence == null)
            {
                return NotFound("Přestupek nebyl nalezen.");
            }

            if (offence.Fine == null)
            {
                return BadRequest("Přestupek nemá žádnou pokutu.");
            }

            offence.Fine.IsActive = false;
            offence.Fine.PaidOn = DateOnly.FromDateTime(DateTime.Now);

            await _context.SaveChangesAsync();
            return Ok("Pokuta byla úspěšně zaplacena.");
        }

        ////////////////// DELETE METHODS //////////////////

        // DELETE: api/Offence/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffence(int id)
        {
            var offence = await _context.Offences.FindAsync(id);
            if (offence == null)
            {
                return NotFound();
            }

            _context.Offences.Remove(offence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OffenceExists(int id)
        {
            return _context.Offences.Any(e => e.OffenceId == id);
        }
    }
}
