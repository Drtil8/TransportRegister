using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs.TheftDTOs
{
    public class TheftDetailDto
    {
        public int TheftId { get; set; }
        public string Address { get; set; }
        public DateTime StolenOn { get; set; }
        public DateTime ReportedOn { get; set; }
        public DateTime? FoundOn { get; set; }
        public DateTime? ReturnedOn { get; set; }
        public string Description { get; set; }
        public bool IsFound { get; set; }
        public bool IsReturned { get; set; }
        public int VehicleId { get; set; }
        public VehicleListItemDto StolenVehicle { get; set; }
        public UserSimpleDto Official { get; set; }
        public UserSimpleDto OfficerReported { get; set; }
        public UserSimpleDto OfficerFound { get; set; }
    }
}
