namespace TransportRegister.Server.DTOs.LicensePlateHistoryDTOs
{
    public class LicensePlateHistoryDto
    {
        public int LicensePlateHistoryId { get; set; }
        public string LicensePlate { get; set; }
        public DateTime ChangedOn { get; set; }
        public int VehicleId { get; set; }
    }
}
