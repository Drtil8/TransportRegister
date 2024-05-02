using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.UserDTOs;

namespace TransportRegister.Server.Controllers
{
    /// <summary>
    /// Controller for managing user based requests.
    /// </summary>
    /// <author> Dominik Pop </author>
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

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns list of all users in db as DTOs. </returns>
        [HttpPost("/api/Users")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// Method for getting user by its id.
        /// </summary>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Returns DTO containing info about user. </returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDetailDto>> GetUser(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Uživatel nebyl nalezen!");
            }

            return Ok(user);
        }

        /// <summary>
        /// Method for registering new user.
        /// </summary>
        /// <param name="userDto"> DTO containing info about new user. </param>
        /// <returns> Returns if action was successful or not. </returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateDto userDto)
        {
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

        /// <summary>
        /// Method for restoring user. Restores user from soft delete and allows him to log in again.
        /// </summary>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Returns if action was successful or not. </returns>
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

        /// <summary>
        /// Method for deleting user. Soft deletes user from db. User can't log in anymore.
        /// </summary>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Returns if action was successful or not. </returns>
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
