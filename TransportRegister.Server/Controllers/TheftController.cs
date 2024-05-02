using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.OffenceDTOs;
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

    ////////////////// POST METHODS //////////////////

    [HttpPost("/api/Thefts")]
    [Produces("application/json")]
    public async Task<IActionResult> GetThefts([FromBody] DtParamsDto dtParams)
    {
        var query = _theftRepository.QueryAllThefts();
        query = _theftRepository.ApplyFilterQueryThefts(query, dtParams);
        int totalRowCount = await query.CountAsync();
        var thefts = await query.Skip(dtParams.Start).Take(dtParams.Size).ToListAsync();

        return new JsonResult(new DtResultDto<TheftListItemDto>
        {
            Data = thefts,
            TotalRowCount = totalRowCount
        });
    }

    [HttpPost("/api/TheftsActive")]
    [Produces("application/json")]
    public async Task<IActionResult> GetTheftsActive([FromBody] DtParamsDto dtParams)
    {
        var query = _theftRepository.QueryActiveThefts();
        query = _theftRepository.ApplyFilterQueryThefts(query, dtParams);
        int totalRowCount = await query.CountAsync();
        var thefts = await query.Skip(dtParams.Start).Take(dtParams.Size).ToListAsync();

        return new JsonResult(new DtResultDto<TheftListItemDto>
        {
            Data = thefts,
            TotalRowCount = totalRowCount
        });
    }

    [HttpPost("/api/Theft/ReportTheft")]
    [Authorize(Roles = "Officer")]
    public async Task<IActionResult> ReportTheft(TheftCreateDto theft)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        int newTheftId = await _theftRepository.CreateTheft(theft, activeUser.Id);
        return Ok(newTheftId);
    }

    [HttpPut("/api/Theft/ReportTheftDiscovery/{theftId}")]
    [Authorize(Roles = "Officer")]
    public async Task<IActionResult> ReportTheftDiscovery(int theftId)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        await _theftRepository.ReportTheftDiscovery(theftId, activeUser.Id);
        return Ok();
    }

    [HttpPut("/api/Theft/ReportTheftReturn/{theftId}")]
    [Authorize(Roles = "Official")]
    public async Task<IActionResult> ReportTheftReturn(int theftId)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        await _theftRepository.ReportTheftReturn(theftId, activeUser.Id);
        return Ok();
    }
}
