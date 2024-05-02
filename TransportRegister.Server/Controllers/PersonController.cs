using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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

        /// POST: api/PersonSearch
        /// <summary>
        /// Search for persons and drivers based on DataTable parameters.
        /// </summary>
        /// <param name="dtParams">DataTable parameters for search.</param>
        /// <returns>A JSON result containing the search results.</returns>
        [HttpPost("/api/PersonSearch")]
        [Produces("application/json")]
        public async Task<IActionResult> PersonAndDriverSearch([FromBody] DtParamsDto dtParams)
        {
            var query = _personRepository.QueryAllPersons(dtParams);
            int totalRowCount = await query.CountAsync();
            var filteredData = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResultDto<DriverSimpleListDto>
            {
                Data = filteredData,
                TotalRowCount = totalRowCount
            });
        }

        /// GET: api/Persons/5
        /// <summary>
        /// Get a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve.</param>
        /// <returns>An ActionResult containing the person information.</returns>
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
        /// GET: api/Persons/{id}/SetPersonToDriver
        /// <summary>
        /// Set a person as a driver.
        /// </summary>
        /// <param name="personId">The ID of the person to set as a driver.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpGet("{personId}/SetToDriver")]
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> SetPersonToDriver(int personId, string license)
        {
            var person = await _context.Persons.FindAsync(personId);
            if (person == null)
            {
                return NotFound();
            }
            person.OfficialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _personRepository.AddDriverAsync(person.PersonId, license);

            return Ok();
        }
        /// GET: api/Persons/5/ReportedThefts
        /// <summary>
        /// Get reported thefts associated with a person.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve thefts for.</param>
        /// <returns>An ActionResult containing reported theft information.</returns>
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
                StolenOn = t.StolenOn,
                FoundOn = t.FoundOn,
                IsFound = t.FoundOn != null,
            });
            return Ok(theftListDto);
        }
        /// GET: api/Persons/5/CommitedOffences
        /// <summary>
        /// Get offences committed by a person.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve offences for.</param>
        /// <returns>An ActionResult containing offence information.</returns>
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

        /// PUT: api/Persons/5
        /// <summary>
        /// Update a person's information.
        /// </summary>
        /// <param name="id">The ID of the person to update.</param>
        /// <param name="person">The updated person information.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
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
        /// POST: api/Persons/Drivers/{id}
        /// <summary>
        /// Update a driver's information.
        /// </summary>
        /// <param name="id">The ID of the driver to update.</param>
        /// <param name="driver">The updated driver information.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
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
        /// PUT: api/Persons/5/RemoveLicenseSuspension
        /// <summary>
        /// Remove license suspension for a driver.
        /// </summary>
        /// <param name="driverId">The ID of the driver to remove suspension for.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
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

        /// POST: api/Persons
        /// <summary>
        /// Add driver's licenses for a person.
        /// </summary>
        /// <param name="driverId">The ID of the driver to add licenses for.</param>
        /// <param name="new_licenses">A list of new licenses to add.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
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
                _ => "Unknown License Type"
            };
        }


        /// Post "{personId}/UploadImage"
        /// <summary>
        /// Upload an image for a person.
        /// </summary>
        /// <param name="personId">The ID of the person to upload an image for.</param>
        /// <param name="file">The image file to upload.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
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
        /// <summary>
        /// Delete a person by ID.
        /// </summary>
        /// <param name="id">The ID of the person to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
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
