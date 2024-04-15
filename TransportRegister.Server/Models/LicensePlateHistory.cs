namespace TransportRegister.Server.Models
{
    public class LicensePlateHistory

    {
        public int LicensePlateHistoryId { get; set; }
        public string LicensePlate { get; set; }
        public DateTime ChangedOn { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
