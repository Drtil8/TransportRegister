using Microsoft.AspNetCore.Identity;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public static class DbSeeder
{
    public static async Task SeedAll(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // Synchronous seeding 
        OwnerSeed.Seed(context);
        VehicleSeed.Seed(context);


        // Asynchronous seeding
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await UserSeed.Seed(userManager);
        
        OffenceSeed.Seed(context);
        TheftSeed.Seed(context);
        //FineSeed.Seed(context);
    }
}
