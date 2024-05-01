using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.Repositories.Implementations;

namespace TransportRegister.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;

        public PersonsController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        // GET: all persons with driver and owner dto data

        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(int id)
        {
            var person = await _personRepository.GetPersonByIdAsync(id);

            if (person == null)
            {
                return NotFound();
            }
            var personDto = PersonDtoTransformer.TransformToDto(person);
            return personDto;
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPerson(int id, string role)
        {
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person is null)
            {
                return BadRequest();
            }

            //switch (role)
            //    {
            //    case "Driver":
            //        await _personRepository.SetDriverAsync(person);
            //        break;
            //    case "Owner":
            //        await _personRepository.SetOwnerAsync(person);
            //        break;
            //    default:
            //        return BadRequest();
            //    }


                return Ok();
        }

        // POST: api/Persons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public Task<ActionResult<Person>> PostPerson(Person person)
        //{
        //    //_context.Persons.Add(person);
        //    //await _context.SaveChangesAsync();

        //    //return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
        //    return NoContent();
        //}

        [HttpPost("{personId}/UploadImage")]
        public async Task<IActionResult> UploadImage(int personId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                return NotFound("Person not found.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                person.Image = memoryStream.ToArray();
            }

            await _personRepository.SavePersonAsync(person);

            return Ok();
        }

        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            await _personRepository.DeletePersonAsync(id);

            return NoContent();
        }

    }
}
