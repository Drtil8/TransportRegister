namespace TransportRegister.Server.Models
{
    public class OffenceType
    {
        public int OffenceTypeId { get; set; }
        public string Name { get; set; }
        public int PenaltyPoints { get; set; }
        public double FineAmount { get; set; }
    }
}
