using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
    public class OffencePhotoSeed
    {
        public static void Seed(AppDbContext context)
        {
            var photosToSeed = new OffencePhoto[]
            {
                new()
                {
                    Image = File.ReadAllBytes("Seeds/Images/Offences/istockphoto-471089928-612x612.jpg"),
                    OffenceId = 2
                },
                new()
                {
                    Image = File.ReadAllBytes("Seeds/Images/Offences/prestupek-parkovani-praha-policie.jpg"),
                    OffenceId = 2
                }
            };

            foreach (var photo in photosToSeed)
            {
                if (!context.OffencePhotos.Any(p => p.OffencePhotoId == photo.OffencePhotoId))
                {
                    context.OffencePhotos.Add(photo);
                }
            }
            context.SaveChanges();
        }
    }
}
