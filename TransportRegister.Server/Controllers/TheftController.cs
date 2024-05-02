using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;

namespace TransportRegister.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TheftController : ControllerBase
{
    private readonly ITheftRepository _theftRepository;
    private readonly UserManager<User> _userManager;

    public TheftController(ITheftRepository theftRepository, UserManager<User> userManager)
    {
        _theftRepository = theftRepository;
        _userManager = userManager;
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
    public async Task<IActionResult> ReportTheft(TheftCreateDto theft)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        int newTheftId = await _theftRepository.CreateTheft(theft, activeUser.Id);
        return Ok(newTheftId);
    }

    [HttpPost("/api/Theft/ReportTheftDiscovery/{theftId}")]
    public async Task<IActionResult> ReportTheftDiscovery(int theftId)
    {
        await _theftRepository.ReportTheftDiscovery(theftId);
        return Ok();
    }
}
