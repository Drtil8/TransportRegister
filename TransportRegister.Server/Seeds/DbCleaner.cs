using TransportRegister.Server.Data;

namespace TransportRegister.Server.Seeds;

public class DbCleaner
{
    public static void ClearAllData(AppDbContext context)
    {
        var vehicles = context.Vehicles.ToList();
        context.Vehicles.RemoveRange(vehicles);

        var owners = context.Owners.ToList();
        context.Owners.RemoveRange(owners);

        context.SaveChanges();
    }
}