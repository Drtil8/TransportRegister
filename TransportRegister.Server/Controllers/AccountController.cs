using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TransportRegister.Server.Models;
using TransportRegister.Server.ViewModels;

namespace TransportRegister.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<User> _signInManager;

    public AccountController(SignInManager<User> signInManager, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        return BadRequest(ModelState);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("IsLoggedIn")]
    public IActionResult IsLoggedIn()
    {
        //var isAdmin = User.IsInRole("Admin");     // todo isAdmin query
        bool isLoggedIn = User.Identity is { IsAuthenticated: true };   // todo why not User.Identity.IsAuthenticated
        string role = null;
        if (isLoggedIn)
            role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            IsLoggedIn = isLoggedIn,
            Email = User.Identity?.Name,
            Role = role
        });
    }
}
