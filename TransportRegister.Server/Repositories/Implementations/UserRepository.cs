using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories.Implementations
{
    public class UserRepository(AppDbContext context, UserManager<User> userManager) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<UserDetailDto> GetUserByIdAsync(string userId)
        {
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(u => u.PersonId == userId);

            //return user == null ? null : new UserDetailDto
            //{
            //    UserId = user.PersonId,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    PhoneNumber = user.PhoneNumber,
            //    Role = user.Role
            //};
            return null; // TODO
        }

        public async Task<UserDetailDto> GetUserByEmailAsync(string email)
        {
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(u => u.Email == email);

            //return user == null ? null : new UserDetailDto
            //{
            //    UserId = user.PersonId,
            //    Email = user.Email,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    PhoneNumber = user.PhoneNumber,
            //    Role = user.Role
            //};
            return null; // TODO
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

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

        public IQueryable<UserListItemDto> GetAllUsers()
        {
            return _context.Users.AsNoTracking()
                .Select(u =>
                new UserListItemDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Role = UserRepository.GetUserRole(u.UserType),
                    IsValid = u.IsValid,
                    IsActive = u.IsActive
                });
        }

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

            query = query.OrderByDescending(p => p.Id);

            return query;
        }

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

        public async Task EditUserAsync(string userId, UserCreateDto userDto)
        {
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(u => u.PersonId == userId);

            //if (user == null)
            //{
            //    throw new Exception("User not found");
            //}

            //user.Email = userDto.Email;
            //user.FirstName = userDto.FirstName;
            //user.LastName = userDto.LastName;
            //user.PhoneNumber = userDto.PhoneNumber;
            //user.Role = userDto.Role;

            //await _context.SaveChangesAsync();
        }

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
