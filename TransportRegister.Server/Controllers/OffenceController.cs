using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffenceController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OffenceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Offence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offence>>> GetOffences()
        {
            return await _context.Offences.ToListAsync();
        }

        // GET: api/Offence/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Offence>> GetOffence(int id)
        {
            var offence = await _context.Offences.FindAsync(id);

            if (offence == null)
            {
                return NotFound();
            }

            return offence;
        }

        // PUT: api/Offence/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffence(int id, Offence offence)
        {
            if (id != offence.OffenceId)
            {
                return BadRequest();
            }

            _context.Entry(offence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OffenceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Offence
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Offence>> PostOffence(Offence offence)
        {
            _context.Offences.Add(offence);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOffence", new { id = offence.OffenceId }, offence);
        }

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
