namespace TransportRegister.Server.Models
{
    public class Fine
    {
        public Guid FineId { get; set; } = Guid.NewGuid();
        public int Amount { get; set; }     // float?
        public bool IsActive { get; set; }
        public DateOnly PaidOn { get; set; }
        public Guid OffenceId { get; set; }
        public Offence IssuedFor { get; set; }
    }
}
