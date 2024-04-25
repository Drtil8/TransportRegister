using Microsoft.AspNetCore.Mvc;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.DTOs.TheftDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories.Implementations;

namespace TransportRegister.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TheftController : ControllerBase
{
    private readonly ITheftRepository _theftRepository;
    public TheftController(ITheftRepository theftRepository)
    {
        _theftRepository = theftRepository;
    }

    [HttpGet("/api/Theft/GetActiveThefts")]
    public async Task<ActionResult<IEnumerable<TheftListItemDto>>> GetActiveThefts()
    {
        var thefts = await _theftRepository.GetActiveThefts();
        return Ok(thefts);
    }

    [HttpGet("/api/Theft/GetTheftById/{theftId}")]
    public async Task<ActionResult<TheftDetailDto>> GetTheftById(int theftId)
    {
        var theft = await _theftRepository.GetTheftById(theftId);
        return Ok(theft);
    }

    [HttpPost("/api/Theft/ReportTheft")]
    public async Task<IActionResult> ReportTheft(TheftDetailDto theft)
    {
        int newTheftId = await _theftRepository.CreateTheft(theft);
        return Ok(newTheftId);
    }

    [HttpPost("/api/Theft/ReportTheftDiscovery/{theftId}")]
    public async Task<IActionResult> ReportTheftDiscovery(int theftId)
    {
        await _theftRepository.ReportTheftDiscovery(theftId);
        return Ok();
    }
}
