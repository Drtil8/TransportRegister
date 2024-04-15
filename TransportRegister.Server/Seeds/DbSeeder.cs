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



        // Asynchronous seeding
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        await UserSeed.Seed(userManager);
                
        // Synchronous seeding 
        PersonSeed.Seed(context);
        VehicleSeed.Seed(context);
        OffenceSeed.Seed(context);
        TheftSeed.Seed(context);
        FineSeed.Seed(context);
        DriversLicenseSeed.Seed(context);
        LicensePlateHistorySeed.Seed(context);
    }
}
