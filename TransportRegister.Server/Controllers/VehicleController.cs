using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories.VehicleRepository;

namespace TransportRegister.Server.Controllers
{
    // todo adjust roles
    //[Authorize]       // All roles can access
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Official")]
    [Authorize(Roles = "Official,Officer")]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }
        
        [HttpGet("VehicleTypes")]
        public async Task<ActionResult<List<string>>> GetVehicleTypes()
        {
            List<string> vehicleTypes = await _vehicleRepository.GetVehicleTypesAsync();

            return Ok(vehicleTypes);
        }

        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(int vehicleId)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
                return NotFound();
            
            VehicleDto vehicleDto = VehicleDtoTransformer.TransformToDto(vehicle);
            if (vehicleDto == null)
                return NotFound("Vehicle type is not supported.");

            return Ok(vehicleDto);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleDto>> SaveVehicle([FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // TODO kontrola zda daný Owner a Official existují
            
            Vehicle vehicle = VehicleDtoTransformer.TransformToEntity(vehicleDto);
            if (vehicle == null)
            {
                return BadRequest("Invalid vehicle data.");
            }

            await _vehicleRepository.SaveVehicleAsync(vehicle);

            VehicleDto updatedDto = VehicleDtoTransformer.TransformToDto(vehicle);
            if (updatedDto == null)
            {
                return NotFound("Failed to update vehicle data.");
            }

            return Ok(updatedDto);
        }
        
        [HttpDelete("{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            Vehicle vehicleExists = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicleExists == null)
            {
                return NotFound();
            }

            await _vehicleRepository.DeleteVehicleAsync(vehicleId);
            return NoContent();
        }
        
        [HttpGet("LicensePlateHistory/{id}")]
        public async Task<ActionResult<List<LicensePlateHistoryDto>>> GetLicensePlateHistory(int id)
        {
            List<LicensePlateHistoryDto> licensePlateHistory = await _vehicleRepository.GetLicensePlateHistoryAsync(id);
            // TODO check if it's useful
            // if (licensePlateHistory == null || licensePlateHistory.Count == 0)
            // {
            //     return NotFound($"No license plate history found for vehicle with ID {id}.");
            // }

            return Ok(licensePlateHistory);
        }
    }
}
