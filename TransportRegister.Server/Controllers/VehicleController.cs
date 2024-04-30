﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.LicensePlateHistoryDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;

namespace TransportRegister.Server.Controllers
{
    // todo when not authorized returns 404 instead of 401 -> create custom error handler in frontend
    // todo adjust roles
    //[Authorize]       // All roles can access
    //[Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Official")]
    //[Authorize(Roles = "Official,Officer")]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IOwnerRepository _ownerRepository;

        public VehicleController(IVehicleRepository vehicleRepository, IOwnerRepository ownerRepository)
        {
            _vehicleRepository = vehicleRepository;
            _ownerRepository = ownerRepository;
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

        [HttpGet("VehicleTypes")]
        public async Task<ActionResult<List<string>>> GetVehicleTypes()
        {
            //var isAdmin = User.IsInRole("Admin");     // todo isAdmin query
            List<string> vehicleTypes = await _vehicleRepository.GetVehicleTypesAsync();

            return Ok(vehicleTypes);
        }

        [HttpGet("{vehicleId}")]
        public async Task<ActionResult<VehicleDetailDto>> GetVehicleById(int vehicleId)
        {
            Vehicle vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
                return NotFound();

            VehicleDetailDto vehicleDto = VehicleDtoTransformer.TransformToDto(vehicle);
            if (vehicleDto == null)
                return NotFound("Vehicle type is not supported.");

            return Ok(vehicleDto);
        }

        [HttpPost("SaveVehicle")]
        public async Task<ActionResult<VehicleDetailDto>> SaveVehicle([FromBody] VehicleDetailDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO kontrola zda daný Official existují

            Vehicle vehicle = VehicleDtoTransformer.TransformToEntity(vehicleDto);
            if (vehicle == null)
            {
                return BadRequest("Invalid vehicle data.");
            }

            vehicle.Owner = await _ownerRepository.GetOwnerByIdAsync(vehicle.OwnerId);
            if (vehicle.Owner is null)
                return BadRequest("Owner not found.");

            // todo when edit must checked if vehicle.LicensePlates.Count == 0 in repository
            // Create new record of license plate history
            vehicle.LicensePlates =
            [
                new LicensePlateHistory
                {
                    LicensePlate = vehicleDto.CurrentLicensePlate,
                    ChangedOn = DateTime.Now
                }
            ];

            await _vehicleRepository.SaveVehicleAsync(vehicle);

            VehicleDetailDto updatedDto = VehicleDtoTransformer.TransformToDto(vehicle);
            if (updatedDto == null)
            {
                return NotFound("Failed to update vehicle data.");
            }

            return Ok(updatedDto);
        }
        
        
        [HttpPost("{vehicleId}/UploadImage")]
        public async Task<IActionResult> UploadImage(int vehicleId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }
            
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                vehicle.Image = memoryStream.ToArray();
            }

            await _vehicleRepository.SaveVehicleAsync(vehicle);

            return Ok();
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
            return Ok();
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
