using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;

namespace TransportRegister.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(AppDbContext context, UserManager<User> userManager, IUserRepository userRepository)
        {
            _context = context;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpPost("/api/Users")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUsers([FromBody] DtParamsDto dtParams)
        {
            var query = _userRepository.GetAllUsers();
            query = _userRepository.ApplyFilterQueryUsers(query, dtParams);
            int totalRowCount = await query.CountAsync();
            var users = await query.Skip(dtParams.Start).Take(dtParams.Size).ToListAsync();

            return new JsonResult(new DtResultDto<UserListItemDto>
            {
                Data = users,
                TotalRowCount = totalRowCount
            });

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateDto userDto)
        {
            // TODO -> password encryption when sending
            if (userDto.PasswordConfirm != userDto.Password)
            {
                return BadRequest("* Hesla se neshodují.");
            }

            var userExists = await _userRepository.UserExistsAsync(userDto.Email);
            if (userExists)
            {
                return BadRequest("* Uživatel s tímto emailem již existuje.");
            }

            User user;
            if (userDto.Role == "Admin")
            {
                user = await _userRepository.SaveUserAsync<User>(userDto, userDto.Role);
            }
            else if (userDto.Role == "Official")
            {
                user = await _userRepository.SaveUserAsync<Official>(userDto, userDto.Role);
            }
            else if (userDto.Role == "Officer")
            {
                user = await _userRepository.SaveUserAsync<Officer>(userDto, userDto.Role);
            }
            else
            {
                return BadRequest("* Neplatná role.");
            }

            if (user == null)
            {
                return BadRequest("* Registrace se nezdařila.");
            }

            return Ok();
        }

        [HttpPut("{userId}/Restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreUser(string userId)
        {
            var result = await _userRepository.RestoreUserAsync(userId);
            if (!result)
            {
                return BadRequest("* Uživatele se nepodařilo obnovit.");
            }

            return Ok();
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            if (!result)
            {
                return BadRequest("* Uživatele se nepodařilo smazat.");
            }

            return Ok();
        }
    }
}
