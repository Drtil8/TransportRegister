namespace TransportRegister.Server.DTOs.DriversLicenseDTOs
{
    public class DriversLicenseDto
    {
        public int DriversLicenseId { get; set; }
        public DateOnly IssuedOn { get; set; }
        public string Description { get; set; }
        public string VehicleType { get; set; }
        public int DriverId { get; set; }
    }
}
