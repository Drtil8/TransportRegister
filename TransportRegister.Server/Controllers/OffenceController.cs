using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public OffenceController(AppDbContext context, IOffenceRepository offenceRepository)
        {
            _context = context;
            _offenceRepository = offenceRepository;
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
            var offence = await _offenceRepository.ReportOffenceAsync(offenceDto);

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
