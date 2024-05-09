using TransportRegister.Server.Data;

namespace TransportRegister.Server.Seeds;

public static class DbCleaner
{
    /// <summary>
    /// Deletes the entire database
    /// </summary>
    /// <param name="context">DbContext</param>
    public static async Task DeleteEntireDb(AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
    }
}
