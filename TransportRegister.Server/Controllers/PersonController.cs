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
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using Microsoft.IdentityModel.Tokens;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;

namespace TransportRegister.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly AppDbContext _context;

        public PersonsController(IPersonRepository personRepository, AppDbContext context)
        {
            _personRepository = personRepository;
            _context = context;
        }

        // GET: all persons with driver and owner dto data
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAllPersons()
        {
            var persons = await _personRepository.GetAllPersons();

            return Ok(persons);
        }

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

        [HttpGet("{personId}/SetToDriver")]
        public async Task<IActionResult> SetPersonToDriver(int personId)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return NotFound();
            }

            await _personRepository.SetDriverAsync(person);

            return Ok();
        }

        [HttpGet("{id}/ReportedThefts")]
        public async Task<ActionResult<TheftListItemDto>> GetReportedThefts(int id)
        {
            var theftsList = await _personRepository.GetPersonReportedTheftsByIdAsync(id);

            if (theftsList is null)
            {
                return NotFound();
            }
            var theftListDto = theftsList.Select(t => new TheftListItemDto
            {
                TheftId = t.TheftId,
                ReportedOn = t.ReportedOn,
                VehicleId = t.VehicleId,
                VIN = t.StolenVehicle.VIN,
                LicensePlate = t.StolenVehicle.LicensePlates.FirstOrDefault().LicensePlate,
                StolenOn = t.StolenOn,
                FoundOn = t.FoundOn,
                IsFound = t.FoundOn != null,
            });
            return Ok(theftListDto);
        }

        [HttpGet("{id}/CommitedOffences")]
        public async Task<ActionResult<TheftListItemDto>> GetCommitedOffences(int id)
        {
            var offencesList = await _personRepository.GetPersonCommitedOffencesByIdAsync(id);

            if (offencesList == null)
            {
                return NotFound();
            }
            var offencesListDto = offencesList.Select(o => new OffenceListSimpleDto
            {
                OffenceId = o.OffenceId,
                ReportedOn = o.ReportedOn,
                Description = o.Description,
                PenaltyPoints = o.PenaltyPoints,
                FineAmount = o.Fine is not null ? o.Fine.Amount : default,
            });
            return Ok(offencesListDto);
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutPerson(int id, PersonUpdateDto person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }
            
            //await _personRepository.SavePersonAsync(PersonDtoTransformer.TransformPersonUpdateToEntity(person));
           
            return Ok();

        }

        [HttpPut("{driverId}/RemoveLicenseSuspension")]
        public async Task<ActionResult> RemoveLicenseSuspenison(int driverId)
        {
            var person = await _personRepository.GetPersonByIdAsync(driverId);

            if (person is not Driver driver)
            {
                return BadRequest($"Person {driverId} is not a driver.");
            }
            if (driver.HasSuspendedLicense) 
            {
                driver.HasSuspendedLicense = false;
                driver.DrivingSuspendedUntil = null;
                await _personRepository.SavePersonAsync(driver);
                return Ok(); 
            }

            return BadRequest("Driver does not have suspended license.");


        }

        // POST: api/Persons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{driverId}/AddDriversLicense")]
        public async Task<IActionResult>PostDriversLicense(int driverId, DriversLicenseCreateDto license)
        {
            if (!ModelState.IsValid)    /// what about bad vehicle type?
            {
                return BadRequest(ModelState);
            }

            var driver = await _personRepository.GetPersonByIdAsync(driverId);

            if (driver is not Driver)
            {
                return BadRequest($"Person {driverId} is not a driver.");
            }
            if (!Enum.IsDefined(typeof(VehicleType), license.VehicleType))
            {
                return BadRequest($"Invalid vehicle type: {license.VehicleType}");
            }

            await _personRepository.AddDriversLicense(driverId, license);
            return Ok();


        }

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
