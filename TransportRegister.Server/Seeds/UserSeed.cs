using Microsoft.AspNetCore.Identity;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public static class UserSeed
{
    public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedOfficial(userManager);
        await SeedOfficer(userManager);
        await SeedAdmin(userManager);
    }

    const string roleAdmin = "Admin";
    const string roleOfficial = "Official";
    const string roleOfficer = "Officer";

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole(roleAdmin));
        await roleManager.CreateAsync(new IdentityRole(roleOfficial));
        await roleManager.CreateAsync(new IdentityRole(roleOfficer));
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
                IsValid = true,
            };
            await userManager.CreateAsync(adminlUser, "Admin123");

            // Add the user role
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin is not null)
            {
                await userManager.AddToRoleAsync(admin, roleAdmin);
            }
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
                LastName = "Èech",
                UserName = officialEmail,
                Email = officialEmail,
                PhoneNumber = officialTelephone,
                EmailConfirmed = true,
                IsValid = true,
                IsActive = true,
            };
            await userManager.CreateAsync(officialUser, "Official123");

            // Add the user role
            var official = await userManager.FindByEmailAsync(officialEmail);
            if (official is not null)
            {
                await userManager.AddToRoleAsync(official, roleOfficial);
            }
        }

        const string officialId2 = "26c13c10-18e6-4967-8ff3-fc9a76421333";
        if (await userManager.FindByIdAsync(officialId2) == null)
        {
            const string officialEmail2 = "official2@example.com";
            const string officialTelephone2 = "+420 789 789";
            var officialUser2 = new Official()
            {
                Id = officialId2,
                FirstName = "Pavel",
                LastName = "Nedvìd",
                UserName = officialEmail2,
                Email = officialEmail2,
                PhoneNumber = officialTelephone2,
                EmailConfirmed = true,
                IsValid = true,
                IsActive = true,
            };
            await userManager.CreateAsync(officialUser2, "Official123");

            var official2 = await userManager.FindByEmailAsync(officialEmail2);
            if (official2 is not null)
            {
                await userManager.AddToRoleAsync(official2, roleOfficial);
            }
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
                IsValid = true,
                IsActive = true,
            };
            await userManager.CreateAsync(officerUser, "Officer123");

            // Add the user role
            var officer = await userManager.FindByEmailAsync(officerEmail);
            if (officer is not null)
            {
                await userManager.AddToRoleAsync(officer, roleOfficer);
            }
        }
    }
}
