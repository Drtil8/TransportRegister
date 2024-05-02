using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    public class OffenceCreateDto
    {
        public string Description { get; set; }
        public int OffenceTypeId { get; set; }
        public int PenaltyPoints { get; set; }
        public AddressDto Address { get; set; }
        public double FineAmount { get; set; }
        public bool FinePaid { get; set; }
        public int VehicleId { get; set; } // Comitted on vehicle
        public int PersonId { get; set; } // Comitted by person -> if created for driver, then it is driver, if created for vechicle then it is owner
        public ICollection<string> Photos { get; set; }
    }
}
