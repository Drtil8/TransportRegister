namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    /// <summary>
    /// TODO - comment
    /// </summary>
    /// <author> Dominik Pop </author>
    public class OffenceVehicleDto
    {
        public int VehicleId { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
    }
}
