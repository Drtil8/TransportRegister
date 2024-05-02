using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    /// <summary>
    /// Repository for user management.
    /// </summary>
    /// <author> Dominik Pop </author>
    public class UserRepository(AppDbContext context, UserManager<User> userManager) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;

        /// <summary>
        /// Checks if user with given email exists.
        /// </summary>
        /// <param name="email"> Email of user. </param>
        /// <returns> Returns true if user exists. </returns>
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        ////////////////// GETTERS //////////////////

        /// <summary>
        /// Gets user by id from the database.
        /// </summary>
        /// <param name="userId"> Users id. </param>
        /// <returns> Returns user as DTO. </returns>
        public async Task<UserDetailDto> GetUserByIdAsync(string userId)
        { 
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var userDto = new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = UserRepository.GetUserRole(user.UserType),
                IsValid = user.IsValid
            };

            if (user is Officer officer)
            {
                userDto.PersonalId = officer.PersonalId;
                userDto.Rank = officer.Rank;
            }

            return userDto;
        }

        /// <summary>
        /// Returns role of the user.
        /// </summary>
        /// <param name="userType"> User type from db. </param>
        /// <returns> Returns string representing users role. </returns>
        public static string GetUserRole(string userType)
        {
            if(userType == "User")
            {
                return "Administrátor";
            }
            else if(userType == "Official")
            {
                return "Úředník";
            }
            else if(userType == "Officer")
            {
                return "Policista";
            }

            return "Neznámý";
        }

        /// <summary>
        /// Returns all users from the database.
        /// </summary>
        /// <returns> Returns query of user DTOs. </returns>
        public IQueryable<UserListItemDto> GetAllUsers()
        {
            return _context.Users.AsNoTracking()
                .Select(u =>
                new UserListItemDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = u.FirstName + " " + u.LastName,
                    Role = u.UserType == "User" ? "Administrátor" :
                           u.UserType == "Official" ? "Úředník" :
                           u.UserType == "Officer" ? "Policista" :
                           "Neznámý",
                    IsValid = u.IsValid,
                    IsActive = u.IsActive
                });
        }

        ////////////////// FILTER //////////////////

        /// <summary>
        /// Applies filters to the query of users.
        /// </summary>
        /// <param name="query"> Query. </param>
        /// <param name="dtParams"> Datatable parametres. </param>
        /// <returns> Returns query with applied filters. </returns>
        public IQueryable<UserListItemDto> ApplyFilterQueryUsers(IQueryable<UserListItemDto> query, DtParamsDto dtParams)
        {
            foreach(var filter in dtParams.Filters)
            {
                query = filter.PropertyName switch
                {
                    nameof(UserListItemDto.Email) => query.Where(u => u.Email.Contains(filter.Value)),
                    nameof(UserListItemDto.FullName) => query.Where(u => u.FullName.Contains(filter.Value)),
                    nameof(UserListItemDto.Role) => query.Where(u => u.Role.Contains(filter.Value)),
                    _ => query
                };
            }

            if (dtParams.Sorting.Any())
            {
                Sorting sorting = dtParams.Sorting.First();
                return query.OrderBy($"{sorting.Id} {sorting.Dir}")
                    .ThenByDescending(v => v.Id);
            }
            else
            {
                return query.OrderByDescending(v => v.Id);
            }
        }

        ////////////////// ACTIONS //////////////////

        /// <summary>
        /// Saves user to the database.
        /// </summary>
        /// <typeparam name="TUser"> Generic which intakes any type of role user can be. </typeparam>
        /// <param name="userDto"> DTO containing info about new user. </param>
        /// <param name="role"> Users role. </param>
        /// <returns> Returns newly created user. </returns>
        public async Task<TUser> SaveUserAsync<TUser>(UserCreateDto userDto, string role) where TUser : User, new()
        {
            var newUser = new TUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = userDto.Email,
                EmailConfirmed = true,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.Email,
                IsActive = true,
                IsValid = true,
            };
            var result = await _userManager.CreateAsync(newUser, userDto.Password);
            if(!result.Succeeded)
            {
                return null;
            }

            result = await _userManager.AddToRoleAsync(newUser, userDto.Role);
            if(!result.Succeeded)
            {
                await _userManager.DeleteAsync(newUser);
                return null;
            }

            return newUser;
        }

        /// <summary>
        /// Restores user to the database. Sets IsValid to true.
        /// User can log in again.
        /// </summary>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Returns true if user was restored. </returns>
        public async Task<bool> RestoreUserAsync(string userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            user.IsValid = true;
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Deletes user from the database. Sets IsValid to false. 
        /// User is not removed from the database, but cannot log in.
        /// </summary>
        /// <param name="userId"> Id of user. </param>
        /// <returns> Returns true if user was "deleted". </returns>
        public async Task<bool> DeleteUserAsync(string userId)
        {

            var user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            user.IsValid = false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
