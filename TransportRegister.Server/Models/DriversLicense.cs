namespace TransportRegister.Server.Models
{
    public enum VehicleType
    {
       AM,
       A1,
       A2,
        A,
        B1,
        B,
        C1,
        C,
        D1,
        D,
        BE,
        C1E,
        CE,
        D1E,
        DE,
        T,
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
