using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.DTOs.TheftDTOs;
using TransportRegister.Server.DTOs.DatatableDTOs;

namespace TransportRegister.Server.Controllers;

/// <summary>
/// Controller for managing theft based requests.
/// </summary>
/// <author> David Drtil </author>
/// <author> Dominik Pop </author>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Officer, Official")]
public class TheftController : ControllerBase
{
    private readonly ITheftRepository _theftRepository;
    private readonly UserManager<User> _userManager;

    public TheftController(ITheftRepository theftRepository, UserManager<User> userManager)
    {
        _theftRepository = theftRepository;
        _userManager = userManager;
    }

    /// <summary>
    /// Method for getting all active thefts.
    /// </summary>
    /// <returns></returns>
    [HttpGet("/api/Theft/GetActiveThefts")]
    public async Task<ActionResult<IEnumerable<TheftListItemDto>>> GetActiveThefts()
    {
        var thefts = await _theftRepository.GetActiveThefts();
        return Ok(thefts);
    }

    /// <summary>
    /// Method for getting theft by its id.
    /// </summary>
    /// <param name="theftId"> Id of theft. </param>
    /// <returns> Returns DTO with information needed for theft detail. </returns>
    [HttpGet("/api/Theft/GetTheftById/{theftId}")]
    public async Task<ActionResult<TheftDetailDto>> GetTheftById(int theftId)
    {
        var theft = await _theftRepository.GetTheftById(theftId);
        return Ok(theft);
    }

    ////////////////// POST METHODS //////////////////

    /// <summary>
    /// Method for getting all thefts.
    /// </summary>
    /// <param name="dtParams"> Datatable parametres. </param>
    /// <returns> Returns thefts in DTOs which will be used for table. </returns>
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

    /// <summary>
    /// Method for getting all active thefts. Theft is active if vehicle was not found and returned to its owner.
    /// </summary>
    /// <param name="dtParams"> Datatable parametres. </param>
    /// <returns> Returns thefts in DTOs which will be used for table. </returns>
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

    /// <summary>
    /// Method for reporting a theft and creating a new theft record.
    /// </summary>
    /// <param name="theft"> DTO creatining information about new theft. </param>
    /// <returns> Returns if action was successful or not. </returns>
    [HttpPost("/api/Theft/ReportTheft")]
    [Authorize(Roles = "Officer")]
    public async Task<ActionResult<int>> ReportTheft(TheftCreateDto theft)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        int newTheftId = await _theftRepository.CreateTheft(theft, activeUser.Id);
        return Ok(newTheftId);
    }

    ////////////////// PUT METHODS //////////////////

    /// <summary>
    /// Method for reporting that the vehicle was found.
    /// </summary>
    /// <param name="theftId"> Id of theft. </param>
    /// <returns> Returns if action was successful or not. </returns>
    [HttpPut("/api/Theft/ReportTheftDiscovery/{theftId}")]
    [Authorize(Roles = "Officer")]
    public async Task<IActionResult> ReportTheftDiscovery(int theftId)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        await _theftRepository.ReportTheftDiscovery(theftId, activeUser.Id);
        return Ok();
    }

    /// <summary>
    /// Method for reporting that the vechicle was returned to its owner.
    /// </summary>
    /// <param name="theftId"> Id of theft. </param>
    /// <returns> Returns if action was successful or not. </returns>
    [HttpPut("/api/Theft/ReportTheftReturn/{theftId}")]
    [Authorize(Roles = "Official")]
    public async Task<IActionResult> ReportTheftReturn(int theftId)
    {
        var activeUser = await _userManager.GetUserAsync(User);
        await _theftRepository.ReportTheftReturn(theftId, activeUser.Id);
        return Ok();
    }
}
