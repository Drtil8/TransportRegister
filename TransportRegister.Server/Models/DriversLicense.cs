namespace TransportRegister.Server.Models
{
    public enum VehicleType
    {
        Car,
        Motorcycle,
        Truck,
        Bus,
        Other
    }
    public class DriversLicense
    {
        public int DriversLicenseId { get; set; }
        public DateOnly IssuedOn { get; set; }
        public string Description { get; set; }

        public VehicleType VehicleType { get; set; }
        public int DriverId { get; set; }
        public Driver IssuedFor { get; set; }
    }
}
