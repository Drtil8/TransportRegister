namespace TransportRegister.Server.DTOs.FineDTOs
{
    public class FineDetailDto
    {
        public int FineId { get; set; }
        public double Amount { get; set; }
        public bool IsActive { get; set; }
        public bool IsPaid { get; set; }
        public DateOnly PaidOn { get; set; }
    }
}
