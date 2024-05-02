namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    public class OffenceTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PenaltyPoints { get; set; }
        public double FineAmount { get; set; }
    }
}
