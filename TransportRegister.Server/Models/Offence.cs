namespace TransportRegister.Server.Models

{
    public class Offence
    {
        public int OffenceId { get; set; }
        public DateTime ReportedOn { get; set; }
        public bool IsValid { get; set; }
        public string Description { get; set; }

        public int? FineId { get; set; }    
        public Fine Fine { get; set; }      // zmazane ? nullable -- otestovať

        public int VehicleId { get; set; }
        public Vehicle OffenceOnVehicle { get; set; }
        public string OfficerId { get; set; }
        public Officer ReportedByOfficer { get; set; }
        public string OfficialId { get; set; }
        public Official ProcessedByOfficial { get; set; }
        public int PersonId { get; set; }
        public Person CommitedBy {  get; set; }
    }
}
