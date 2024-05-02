using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;

namespace TransportRegister.Server.Controllers
{
    /// <summary>
    /// Controller for handling vehicle related data operations such as retrieval, modification, and deletion.
    /// </summary>
    [Authorize(Roles = "Official,Officer")]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPersonRepository _personRepository;
        
        public VehicleController(IVehicleRepository vehicleRepository, IPersonRepository ownerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _personRepository = ownerRepository;
        }

        /// <summary>
        /// Get filtered vehicles
        /// </summary>
        /// <param name="dtParams">Data table filtering parameters</param>
        /// <returns>Filtered vehicles DTOs</returns>
        [HttpPost("/api/VehicleSearch")]
        [Produces("application/json")]
        public async Task<IActionResult> VehicleSearch([FromBody] DtParamsDto dtParams)
        {
            var query = _vehicleRepository.QueryVehicleSearch(dtParams);
            int totalRowCount = await query.CountAsync();
            var filteredData = await query
                .Skip(dtParams.Start)
                .Take(dtParams.Size)
                .ToListAsync();
            return new JsonResult(new DtResultDto<VehicleListItemDto>
            {
                Data = filteredData,
                TotalRowCount = totalRowCount
            });
        }
        
        /// <summary>
        /// Retrieves a list of all vehicle types available in the system.
        /// </summary>
        /// <returns>List of vehicle types</returns>
        [HttpGet("VehicleTypes")]
        public async Task<ActionResult<List<string>>> GetVehicleTypes()
        {
            //var isAdmin = User.IsInRole("Admin");     // todo isAdmin query
            List<string> vehicleTypes = await _vehicleRepository.GetVehicleTypesAsync();

            return Ok(vehicleTypes);
        }

        /// <summary>
        /// Retrieves detailed information about a specific vehicle by its ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to retrieve</param>
        /// <returns>Vehicle details if found, otherwise returns NotFound result</returns>
        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<VehicleDetailDto>> GetVehicleById(int vehicleId)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
                return NotFound();

            VehicleDetailDto vehicleDto = VehicleDtoTransformer.TransformToDto(vehicle);
            if (vehicleDto == null)
                return NotFound("Vehicle type is not supported.");

            vehicleDto.LicensePlates = await _vehicleRepository.GetLicensePlateHistoryAsync(vehicleId);

            return Ok(vehicleDto);
        }

        /// <summary>
        /// Saves a vehicle to the database. If the vehicle is new, it will be added. If the vehicle exists, it will be updated.
        /// </summary>
        /// <param name="vehicleDto">Vehicle data transfer object containing vehicle details to save or update</param>
        /// <returns>Returns BadRequest if model state is invalid, otherwise returns updated vehicle details</returns>
        [Authorize(Roles = "Official")]
        [HttpPost("SaveVehicle")]
        public async Task<ActionResult<VehicleDetailDto>> SaveVehicle([FromBody] VehicleDetailDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Vehicle vehicle = VehicleDtoTransformer.TransformToEntity(vehicleDto);
            if (vehicle == null)
            {
                return BadRequest("Invalid vehicle data.");
            }

            vehicle.OfficialId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            vehicle.Owner = await _personRepository.GetPersonByIdAsync(vehicleDto.OwnerId);
            if (vehicle.Owner is null)
                return BadRequest("Owner not found.");

            var existingPlateHistoryDto = await _vehicleRepository.GetLicensePlateHistoryAsync(vehicle.VehicleId);
            var existingPlateHistory = LicenseHistoryDtoTransformer.ConvertDtoToLicensePlateHistory(existingPlateHistoryDto);
            if (!existingPlateHistory.Any() || existingPlateHistory.Last().LicensePlate != vehicleDto.CurrentLicensePlate)
            {
                var newLicensePlate = new LicensePlateHistory
                {
                    LicensePlate = vehicleDto.CurrentLicensePlate,
                    ChangedOn = DateTime.Now,
                    Vehicle = vehicle
                };
                existingPlateHistory.Add(newLicensePlate);
            }

            vehicle.LicensePlates = existingPlateHistory;

            await _vehicleRepository.SaveVehicleAsync(vehicle);

            VehicleDetailDto updatedDto = VehicleDtoTransformer.TransformToDto(vehicle);
            if (updatedDto == null)
            {
                return NotFound("Failed to update vehicle data.");
            }
            updatedDto.LicensePlates = await _vehicleRepository.GetLicensePlateHistoryAsync(updatedDto.VehicleId);
            
            var vehicleTmp = await _vehicleRepository.GetVehicleByIdAsync(vehicle.VehicleId);
            updatedDto.OfficialFullName = vehicleTmp.AddedByOfficial.FirstName + " " + vehicleTmp.AddedByOfficial.LastName;
            
            return Ok(updatedDto);
        }

        /// <summary>
        /// Deletes a vehicle from the database based on the provided vehicle ID.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to delete</param>
        /// <returns>Returns NotFound if vehicle does not exist, otherwise returns Ok on successful deletion</returns>
        [HttpDelete("{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            Vehicle vehicleExists = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicleExists == null)
            {
                return NotFound();
            }

            await _vehicleRepository.DeleteVehicleAsync(vehicleId);
            return Ok();
        }

        /// <summary>
        /// Retrieves the license plate history for a specific vehicle by its ID.
        /// </summary>
        /// <param name="id">Vehicle ID to fetch license plate history for</param>
        /// <returns>List of license plate history records</returns>
        [HttpGet("LicensePlateHistory/{id}")]
        public async Task<ActionResult<List<LicensePlateHistoryDto>>> GetLicensePlateHistory(int id)
        {
            List<LicensePlateHistoryDto> licensePlateHistory = await _vehicleRepository.GetLicensePlateHistoryAsync(id);

            return Ok(licensePlateHistory);
        }
    }
}
