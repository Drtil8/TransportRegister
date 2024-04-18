using TransportRegister.Server.Data;

namespace TransportRegister.Server.Seeds;

public static class DbCleaner
{
    /// <summary>
    /// Deletes the entire database
    /// </summary>
    /// <param name="context">DbContext</param>
    public static void DeleteEntireDb(AppDbContext context)
    {
        context.Database.EnsureDeleted();
    }
}
