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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TransportRegister.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Officer,Official")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonRepository _personRepository;
        private readonly AppDbContext _context;

        public PersonsController(IPersonRepository personRepository, AppDbContext context)
        {
            _personRepository = personRepository;
            _context = context;
        }

        [HttpPost("/api/PersonSearch")]
        [Produces("application/json")]
        public async Task<IActionResult> PersonAdnDriverSearch([FromBody] DtParamsDto dtParams)
        {
            var query = _personRepository.QueryPersonAndDriverSearch(dtParams);
            int totalPersonRowCount =  query.Item1.Count();
            int totalDriversRowCount =  query.Item2.Count();
            var filteredPersonData =  query.Item1
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToList();

            var filteredDriverData = query.Item2
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToList();
            var resultPerson = new DtResultDto<PersonSimpleListDto>
            {
                Data = filteredPersonData,
                TotalRowCount = totalPersonRowCount
            };

            var resultDriver = new DtResultDto<DriverSimpleListDto>
            {
                Data = filteredDriverData,
                TotalRowCount = totalDriversRowCount
            };

            var combinedResult = new
            {
                Persons = resultPerson,
                Drivers = resultDriver
            };

            return new JsonResult(combinedResult);
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
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> SetPersonToDriver(int personId)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return NotFound();
            }
            person.OfficialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
                Vehicle = new VehicleSimpleDto
                {
                    Manufacturer = t.StolenVehicle.Manufacturer,
                    Model = t.StolenVehicle.Model,
                    VehicleId = t.VehicleId,
                    VIN = t.StolenVehicle.VIN,
                    LicensePlate = t.StolenVehicle.LicensePlates.FirstOrDefault().LicensePlate,
                },
                //VehicleId = t.VehicleId,
                //VIN = t.StolenVehicle.VIN,
                //LicensePlate = t.StolenVehicle.LicensePlates.FirstOrDefault().LicensePlate,
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
        [HttpPost("{id}")]
        [Authorize(Roles = "Official")]
        public async Task<ActionResult> PutPerson(int id, PersonUpdateDto person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            person.OfficialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _personRepository.SavePersonAsync(PersonDtoTransformer.TransformPersonUpdateToEntity(person));

            return Ok();

        }

        [HttpPost("Drivers/{id}")]
        [Authorize(Roles = "Official")]
        public async Task<ActionResult> PutDriver(int id, DriverUpdateDto driver)
        {
            if (id != driver.PersonId)
            {
                return BadRequest();

            }

            var person = await _personRepository.GetPersonByIdAsync(id);

            if (person is not Driver)
            {
                return BadRequest($"Person {id} is not a driver.");
            }
            driver.OfficialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _personRepository.SaveDriverAsync(PersonDtoTransformer.TransformPersonUpdateToEntity(driver));

            return Ok();

        }


        [HttpPut("{driverId}/RemoveLicenseSuspension")]
        [Authorize(Roles = "Official")]
        public async Task<ActionResult> RemoveLicenseSuspenison(int driverId)
        {
            var person = await _personRepository.GetPersonByIdAsync(driverId);

            if (person is not Driver driver)
            {
                return BadRequest($"Person {driverId} is not a driver.");
            }
            if (driver.HasSuspendedLicense)
            {
                driver.OfficialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> PostDriversLicense(int driverId, List<string> new_licenses)
        {
            if (!ModelState.IsValid)    /// what about bad vehicle type?
            {
                return BadRequest(ModelState);
            }

            var person = await _personRepository.GetPersonByIdAsync(driverId);

            if (person is not Driver driver)
            {
                return BadRequest($"Person {driverId} is not a driver.");
            }

            var officialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (var licenseType in new_licenses)
            {
                if (!Enum.TryParse<VehicleType>(licenseType, out VehicleType vehicleType))
                {
                    return BadRequest($"Invalid vehicle type: {licenseType}");
                }

                var license = new DriversLicenseCreateDto
                {
                    VehicleType = licenseType,
                    IssuedOn = DateOnly.FromDateTime(DateTime.Now),
                    Description = GenerateDescription(licenseType)
                };

                await _personRepository.AddDriversLicense(driverId, officialId, license);
            }

            return Ok();
        }

        static string GenerateDescription(string licenseType)
        {
            return licenseType switch
            {
                "AM" => "Řidičák na mopedy",
                "A1" => "Řidičák na motocykly s obsahem do 125 cm³",
                "A2" => "Řidičák na motocykly s výkonem do 35 kW",
                "A" => "Řidičák na motocykly",
                "B1" => "Řidičák na čtyřkolkové čtyřkolky",
                "B" => "Řidičák na osobní automobily",
                "C1" => "Řidičák na malé nákladní automobily",
                "C" => "Řidičák na nákladní automobily",
                "D1" => "Řidičák na malé autobusy",
                "D" => "Řidičák na autobusy",
                "BE" => "Řidičák na obytné automobily s přívěsem",
                "C1E" => "Řidičák na malé nákladní automobily s přívěsem",
                "CE" => "Řidičák na nákladní automobily s přívěsem",
                "D1E" => "Řidičák na malé autobusy s přívěsem",
                "DE" => "Řidičák na autobusy s přívěsem",
                "T" => "Řidičák na traktor",
                _ => "Unknown License Type" // Default case
            };
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
