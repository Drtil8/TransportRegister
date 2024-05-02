using TransportRegister.Server.DTOs.FineDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    /// <summary>
    /// TODO - comment
    /// </summary>
    /// <author> Dominik Pop </author>
    public class OffenceDetailDto
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        //public string Location { get; set; } // TODO -> use address
        public string Type { get; set; } // TODO -> enum or new table in db
        public bool IsValid { get; set; }
        public bool IsApproved { get; set; }
        public string Description { get; set; }
        public VehicleSimpleDto Vehicle { get; set; }
        public PersonSimpleDto Person { get; set; }
        public UserSimpleDto Officer { get; set; }
        public UserSimpleDto Official { get; set; }
        public int PenaltyPoints { get; set; }
        public FineDetailDto Fine { get; set; }
        public bool IsResponsibleOfficial { get; set; }
        public ICollection<string> OffencePhotos64 { get; set; } = new List<string>();
    }
}
