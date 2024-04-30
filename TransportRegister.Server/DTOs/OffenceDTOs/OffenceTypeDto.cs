namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    /// <summary>
    /// TODO - comment
    /// </summary>
    /// <author> Dominik Pop </author>
    public class OffenceTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PenaltyPoints { get; set; }
        public double FineAmount { get; set; }
    }
}
