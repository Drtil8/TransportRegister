namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    /// <summary>
    /// TODO - comment
    /// </summary>
    /// <author> Dominik Pop </author>
    public class OffenceListItemDto
    {
        public int OffenceId { get; set; }
        //public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public DateTime ReportedOn { get; set; }
        public string OffenceType { get; set; } // TODO 
        public bool IsPaid { get; set; }
        public double Amount { get; set; }
    }
}
