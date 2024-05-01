using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    public class OffenceListSimpleDto
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        public string Description { get; set; }
        public int PenaltyPoints { get; set; }
        public double FineAmount { get; set; }

    }
}
