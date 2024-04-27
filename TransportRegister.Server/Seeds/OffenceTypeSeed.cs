using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
    public class OffenceTypeSeed
    {
        public static void Seed(AppDbContext context)
        {
            var TypesToSeed = new OffenceType[] {
                new()
                {
                    Name = "Neklasifikován"
                },
                new()
                {
                    Name = "Překročení rychlosti"
                },
                new()
                {
                    Name = "Technicky nezpůsobilé vozidlo"
                },
                new()
                {
                    Name = "Chybějící doklady"
                },
                new()
                {
                    Name = "Telefonování za jízdy"
                },
                new()
                {
                    Name = "Jízda na červenou"
                },
                new()
                {
                    Name = "Ohrožení chodce"
                },
                new()
                {
                    Name = "Zakázané předjíždění"
                },
                new()
                {
                    Name = "Nedání přednosti v jízdě"
                }
            };

            foreach (var type in TypesToSeed)
            {
                context.OffenceTypes.Add(type);
            }
            context.SaveChanges();
        }
    }
}
