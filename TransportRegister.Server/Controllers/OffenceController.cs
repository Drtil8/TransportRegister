using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;

namespace TransportRegister.Server.Controllers
{
    /// <summary>
    /// Controller for managing offence based requests.
    /// </summary>
    /// <author> Dominik Pop </author>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Officer, Official")]
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

        /// <summary>
        /// Method for getting offence by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> DTO with offence information. </returns>
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

        /// <summary>
        /// Method for getting types of offences into the report offence modal.
        /// </summary>
        /// <returns> List DTOs containing info needed for select. </returns>
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

        /// <summary>
        /// Method for getting unresolved offences. Offences that are not approved or declined.
        /// </summary>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns list of all offences as DTOs. </returns>
        [HttpPost("/api/Offence/Unresolved")]
        [Produces("application/json")]
        //[Authorize(Roles = "Official")]
        public async Task<IActionResult> GetUnresolvedOffences([FromBody] DtParamsDto dtParams)
        {
            var query = _offenceRepository.QueryUnresolvedOffences();
            //var query = _offenceRepository.QueryOffences(true);
            query = _offenceRepository.ApplyFilterQueryOffences(query, dtParams);
            int totalRowCount = await query.CountAsync();
            var offences = await query.Skip(dtParams.Start).Take(dtParams.Size).ToListAsync();

            return new JsonResult(new DtResultDto<OffenceListItemDto>
            {
                Data = offences,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// Method for getting all offences.
        /// </summary>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns list of all offences as DTOs. </returns>
        [HttpPost("/api/Offences")]
        [Produces("application/json")]
        public async Task<IActionResult> GetOffences([FromBody] DtParamsDto dtParams)
        {
            var query = _offenceRepository.QueryAllOffences();
            //var query = _offenceRepository.QueryOffences(false);
            query = _offenceRepository.ApplyFilterQueryOffences(query, dtParams);
            int totalRowCount = await query.CountAsync();
            var offences = await query.Skip(dtParams.Start).Take(dtParams.Size).ToListAsync();

            return new JsonResult(new DtResultDto<OffenceListItemDto>
            {
                Data = offences,
                TotalRowCount = totalRowCount
            });
        }

        /// <summary>
        /// Method for reporting offence and creating a new offence record.
        /// </summary>
        /// <param name="offenceDto"> DTO containing info about new offence. </param>
        /// <returns> Returns if action was succesfull or not together with its Id. </returns>
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

            return Ok(offence.OffenceId);
        }

        /// <summary>
        /// Method for getting vehicle for report offence modal. Used for searching vehicle by VIN or license plate.
        /// </summary>
        /// <param name="vehicleDto"> DTO contatining SPZ or VIN. </param>
        /// <returns> Returns DTO containing info about chosen vehicle used for modal. </returns>
        [HttpPost("GetVehicleForReport")]
        [Authorize(Roles = "Officer")]
        public async Task<ActionResult<OffenceVehicleDto>> GetVehicleForReport(OffenceVehicleDto vehicleDto)
        {
            Vehicle vehicle;
            if(vehicleDto.LicensePlate != "") // Search by license plate
            {
                var licensePlate = await _context.LicensePlates.Where(lp => lp.LicensePlate == vehicleDto.LicensePlate).Include(lp => lp.Vehicle).FirstOrDefaultAsync();
                if (licensePlate == null)
                {
                    return NotFound("* SPZ nenalezena v systému.");
                }
                vehicle = licensePlate.Vehicle;
            }
            else // Search by VIN
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

        /// <summary>
        /// Method for updating offence by its id.
        /// </summary>
        /// <param name="id"> Id of offence. </param>
        /// <param name="offenceDto"> Offence containing information to be updated. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPut("{id}")]
        //[Authorize(Roles = "Official")]
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
            offence.Description = offenceDto.Description;

            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Method for approving offence by its id. Marks offence as approved.
        /// </summary>
        /// <param name="id"> Id of offence. </param>
        /// <param name="offenceDto"> Returns DTO used for updating info on page. </param>
        /// <returns></returns>
        [HttpPut("{id}/Approve")]
        [Authorize(Roles = "Official")]
        public async Task<ActionResult<OffenceDetailDto>> ApproveOffence(int id, OffenceDetailDto offenceDto)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            var result = await _offenceRepository.ApproveOffenceAsync(id, activeUser.Id, offenceDto);
            if (!result)
            {
                return BadRequest("Přestupek se nepodařilo schválit.");
            }

            return Ok(offenceDto);
        }

        /// <summary>
        /// Method for declining offence by its id. Marks offence as declined.
        /// </summary>
        /// <param name="offenceId"> Id of offence. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPut("{offenceId}/Decline")]
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> DeclineOffence(int offenceId)
        {
            var activeUser = await _userManager.GetUserAsync(User);
            var result = await _offenceRepository.DeclineOffenceAsync(offenceId, activeUser.Id);
            if (!result)
            {
                return BadRequest("Přestupek se nepodařilo zamítnout.");
            }

            return Ok("Přestupek byl úspěšně zamítnut.");
        }

        ////////////////// DELETE METHODS //////////////////

        /// <summary>
        /// Method for deleting offence by its id. Not used.
        /// </summary>
        /// <param name="id"> Id of offence. </param>
        /// <returns> Returns if action was successful or not. </returns>
        // DELETE: api/Offence/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Official")]
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
    }
}
