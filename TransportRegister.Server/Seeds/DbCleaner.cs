using Microsoft.AspNetCore.Identity;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public static class DbCleaner
{
    public static async Task ClearAllData(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var vehicles = context.Vehicles.ToList();
        context.Vehicles.RemoveRange(vehicles);

        var owners = context.Owners.ToList();
        context.Owners.RemoveRange(owners);

        await context.SaveChangesAsync();

        var roles = roleManager.Roles.ToList();
        foreach (var role in roles)
        {
            await roleManager.DeleteAsync(role);
        }

        var users = userManager.Users.ToList();
        foreach (var user in users)
        {
            await userManager.DeleteAsync(user);
        }
    }

}