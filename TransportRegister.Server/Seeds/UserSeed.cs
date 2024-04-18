using Microsoft.AspNetCore.Identity;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public static class UserSeed
{
    public static async Task Seed(UserManager<User> userManager)
    {
        await SeedOfficial(userManager);
        await SeedOfficer(userManager);
        await SeedAdmin(userManager);
    }
    private static async Task SeedAdmin(UserManager<User> userManager)
    {
        const string adminID = "d6f46418-2222-4444-bbbb-162fb5e3a999";
        if (await userManager.FindByIdAsync(adminID) == null)
        {
            const string adminEmail = "admin@example.com";
            const string adminTelephone = "+420 123 123";
            var adminlUser = new User()
            {
                Id = adminID,
                FirstName = "Petr",
                LastName = "Admin",
                UserName = adminEmail,
                Email = adminEmail,
                PhoneNumber = adminTelephone,
                EmailConfirmed = true,
                IsActive = true,
                IsAdmin = true,
                IsValid = true,
            };

            await userManager.CreateAsync(adminlUser, "Admin123");
        }
    }
    private static async Task SeedOfficial(UserManager<User> userManager)
    {
        const string officialId = "d6f46418-2c21-43f8-b167-162fb5e3a999";
        if (await userManager.FindByIdAsync(officialId) == null)
        {
            const string officialEmail = "official@example.com";
            const string officialTelephone = "+420 456 456";
            var officialUser = new Official()
            {
                Id = officialId,
                FirstName = "Petr",
                LastName = "�ech",
                UserName = officialEmail,
                Email = officialEmail,
                PhoneNumber = officialTelephone,
                EmailConfirmed = true,
                IsAdmin = false,
                IsValid = false,
                IsActive = true,
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
            const string officerTelephone = "+420 123 456";
            var officerUser = new Officer()
            {
                Id = officerId,
                FirstName = "Petr",
                LastName = "Pavel",
                UserName = officerEmail,
                Email = officerEmail,
                PhoneNumber = officerTelephone,
                EmailConfirmed = true,
                PersonalId = 123456789,
                Rank = "President",
                IsAdmin = false,
                IsValid = false,
                IsActive = true,
            };

            await userManager.CreateAsync(officerUser, "Officer123");
        }
    }
}