namespace TransportRegister.Server.Models
{
    public class Fine
    {
        public int FineId { get; set; }
        public double Amount { get; set; } 
        public bool IsActive { get; set; }
        public DateOnly PaidOn { get; set; }
        public int OffenceId { get; set; }
        public Offence IssuedFor { get; set; }
    }
}
