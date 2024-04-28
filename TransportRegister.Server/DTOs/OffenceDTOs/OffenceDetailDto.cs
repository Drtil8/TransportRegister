using TransportRegister.Server.DTOs.FineDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    public class OffenceDetailDto
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        //public string Location { get; set; } // TODO -> use address
        public string Type { get; set; } // TODO -> enum or new table in db
        public bool IsValid { get; set; }
        public bool IsApproved { get; set; }
        public string Description { get; set; }
        public VehicleListItemDto Vehicle { get; set; } // TODO -> mby do smaller dto when vehicle is just used for display
        //TODO -> add personDto
        // TODO -> officerDto
        // TODO -> officialDto ??
        public int PenaltyPoints { get; set; }
        public FineDetailDto Fine { get; set; }
    }
}
