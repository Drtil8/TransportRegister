namespace TransportRegister.Server.Models
{
    public enum VehicleClass
    {
        Car,
        Motorcycle,
        Truck,
        Bus,
        Other
    }
    public class DriversLicense
    {
        public Guid DriversLicenseId { get; set; } = Guid.NewGuid();
        public DateOnly IssuedOn { get; set; }
        public string Description { get; set; }

        public VehicleClass VehicleClass { get; set; }
        public Guid DriverId { get; set; }
        public Driver IssuedFor { get; set; }
    }
}
