using TransportRegister.Server.DTOs.DatatableDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Repositories
{
    public interface IUserRepository
    {
        Task<UserDetailDto> GetUserByIdAsync(string userId);
        Task<bool> UserExistsAsync(string email);
        IQueryable<UserListItemDto> GetAllUsers();
        IQueryable<UserListItemDto> ApplyFilterQueryUsers(IQueryable<UserListItemDto> query, DtParamsDto dtParams);
        Task<TUser> SaveUserAsync<TUser>(UserCreateDto userDto, string roleName) where TUser : User, new();
        Task<bool> RestoreUserAsync(string userId);
        Task<bool> DeleteUserAsync(string userId);
    }
}
