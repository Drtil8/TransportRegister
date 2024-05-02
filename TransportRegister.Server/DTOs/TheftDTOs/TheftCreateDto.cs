namespace TransportRegister.Server.DTOs.TheftDTOs
{
    public class TheftCreateDto
    {
        public string Description { get; set; }
        public DateTime StolenOn { get; set; }
        public AddressDto Address { get; set; }
        public int VehicleId { get; set; }
        public int ReportingPersonId { get; set; }
    }
}
