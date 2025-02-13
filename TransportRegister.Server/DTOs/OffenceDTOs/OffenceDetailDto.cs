﻿using TransportRegister.Server.DTOs.FineDTOs;
using TransportRegister.Server.DTOs.PersonDTOs;
using TransportRegister.Server.DTOs.UserDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;

namespace TransportRegister.Server.DTOs.OffenceDTOs
{
    public class OffenceDetailDto
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
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
