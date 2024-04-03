using Microsoft.AspNetCore.Identity;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public static class UserSeed
{
    public static async Task Seed(UserManager<User> userManager)
    {
        await SeedOfficial(userManager);
        await SeedOfficer(userManager);
    }

    private static async Task SeedOfficial(UserManager<User> userManager)
    {
        const string officialId = "d6f46418-2c21-43f8-b167-162fb5e3a999";
        if (await userManager.FindByIdAsync(officialId) == null)
        {
            const string officialEmail = "official@example.com";
            var officialUser = new Official()
            {
                UserName = officialEmail,
                Email = officialEmail,
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(officialUser, "Official123");
        }
    }

    private static async Task SeedOfficer(UserManager<User> userManager)
    {
        const string officerId = "39123a3c-3ce3-4bcc-8887-eb7d8e975ea8";
        if (await userManager.FindByIdAsync(officerId) == null)
        {
            const string officerEmail = "officer@example.com";
            var officerUser = new Officer()
            {
                UserName = officerEmail,
                Email = officerEmail,
                EmailConfirmed = true,
                PersonalId = 123456789,
                Rank = "Captain"
            };

            await userManager.CreateAsync(officerUser, "Officer123");
        }
    }
}