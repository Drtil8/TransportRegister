using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    /// <summary>
    /// TODO - comment
    /// </summary>
    /// <author> Dominik Pop </author>
    public class OffenceListItemDto
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        public string OffenceType { get; set; }
        public bool IsValid { get; set; }
        public bool IsApproved { get; set; }
        public int PenaltyPoints { get; set; }
        public VehicleSimpleDto Vehicle { get; set; }
        public PersonSimpleDto Person { get; set;}
        //public PersonListItemDto Person { get; set; } // todo
    }
}
