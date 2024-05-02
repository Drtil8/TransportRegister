namespace TransportRegister.Server.Models
{
    public class OffencePhoto
    {
        public int OffencePhotoId { get; set; }
        public byte[] Image { get; set; } = default!;

        public int OffenceId { get; set; }
        public Offence Offence { get; set; }
    }
}
