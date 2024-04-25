﻿namespace TransportRegister.Server.Models
{
    public class Offence
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        public bool IsValid { get; set; }
        public bool IsApproved { get; set; } // TODO -> think about this -> we would not delete denied offences but leave them in the memory so they can be checked?
        public string Description { get; set; }
        public int PenaltyPoints { get; set; } // How many penalty points will be assigned
        public Address Address { get; set; } // Place where the offence was commited

        public int? FineId { get; set; }
        public Fine Fine { get; set; }      // zmazane ? nullable -- otestovať
        public int VehicleId { get; set; }
        public Vehicle OffenceOnVehicle { get; set; }
        public string OfficerId { get; set; }
        public Officer ReportedByOfficer { get; set; }
        public string OfficialId { get; set; }
        public Official ProcessedByOfficial { get; set; }
        public int PersonId { get; set; }
        public Person CommitedBy { get; set; }
    }
}
