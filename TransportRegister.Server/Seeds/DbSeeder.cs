using TransportRegister.Server.Data;

namespace TransportRegister.Server.Seeds;

public class DbSeeder
{
    public static void SeedAll(AppDbContext context)
    {
        VehicleSeed.Seed(context);
    }
}