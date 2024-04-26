namespace TransportRegister.Server.DTOs.TheftDTOs
{
    public class TheftListItemDto
    {
        public int TheftId { get; set; }
        public int VehicleId { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public DateTime StolenOn { get; set; }
        public DateTime ReportedOn { get; set; }
        public DateTime? FoundOn { get; set; }
        public bool IsFound { get; set; }
    }
}
