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
                    Name = "Neklasifikován",
                    PenaltyPoints = 0,
                    FineAmount = 0
                },
                new()
                {
                    Name = "Překročení rychlosti",
                    PenaltyPoints = 2,
                    FineAmount = 1000
                },
                new()
                {
                    Name = "Technicky nezpůsobilé vozidlo",
                    PenaltyPoints = 0,
                    FineAmount = 2000
                },
                new()
                {
                    Name = "Chybějící doklady",
                    PenaltyPoints = 3,
                    FineAmount = 3000
                },
                new()
                {
                    Name = "Telefonování za jízdy",
                    PenaltyPoints = 2,
                    FineAmount = 1000
                },
                new()
                {
                    Name = "Jízda na červenou",
                    PenaltyPoints = 5,
                    FineAmount = 2500
                },
                new()
                {
                    Name = "Ohrožení chodce",
                    PenaltyPoints = 4,
                    FineAmount = 2500
                },
                new()
                {
                    Name = "Zakázané předjíždění",
                    PenaltyPoints = 7,
                    FineAmount = 5000
                },
                new()
                {
                    Name = "Nedání přednosti v jízdě",
                    PenaltyPoints = 4,
                    FineAmount = 2500
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
