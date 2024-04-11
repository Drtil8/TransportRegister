namespace TransportRegister.Server.Models
{
    public class Fine
    {
        public Guid FineId { get; set; } = Guid.NewGuid();
        public int Amount { get; set; }     // float?
        public DateOnly PaidOn { get; set; } // either this null or is_active
        public Guid OffenceId { get; set; }
        public Offence IssuedFor { get; set; }
    }
}
