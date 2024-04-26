using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.OffenceDTOs;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<OffenceDetailDto>> GetOffence(int id)
        {
            var offence = await _offenceRepository.GetOffenceByIdAsync(id);

            if (offence == null)
            {
                return NotFound("Hledaný přestupek neexistuje.");
            }

            return Ok(offence);
        }

        ////////////////// POST METHODS //////////////////

        // POST: api/Offence
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        ////////////////// PUT METHODS //////////////////

        // PUT: api/Offence/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffence(int id, OffenceCreateDto offence)
        {

            //_context.Entry(offence).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!OffenceExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            return Ok();
        }

        [HttpPut("{id}/Approve")]
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> ApproveOffence(int id, OffenceCreateDto offenceDto)
        {
            var result = await _offenceRepository.ApproveOffenceAsync(id, offenceDto);
            if (!result)
            {
                return BadRequest("Přestupek se nepodařilo schválit.");
            }

            return Ok("Přestupek byl úspěšně schálen.");
        }

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
