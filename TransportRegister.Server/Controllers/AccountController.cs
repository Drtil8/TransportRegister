using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportRegister.Server.ViewModels;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(SignInManager<User> signInManager) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
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
        await signInManager.SignOutAsync();
        return Ok();
    }
}