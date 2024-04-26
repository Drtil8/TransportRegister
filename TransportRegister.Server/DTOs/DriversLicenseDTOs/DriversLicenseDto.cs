using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.DriversLicenseDTOs
{
    public enum VehicleType
    {
        Car,
        Motorcycle,
        Truck,
        Bus,
        Other
    }

    public class DriversLicenseDto
    {
        public int DriversLicenseId { get; set; }
        public DateOnly IssuedOn { get; set; }
        public string Description { get; set; }
        public VehicleType VehicleType { get; set; }
        public int DriverId { get; set; }
    }
}
