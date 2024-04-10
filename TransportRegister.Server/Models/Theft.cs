namespace TransportRegister.Server.Models
{
    public class Theft
    {
        public Guid TheftId { get; set; } = Guid.NewGuid();
        public DateTime StolenOn { get; set; }
        public DateTime ReportedOn { get; set; }
        public DateTime FoundOn { get; set; }
        public string Description { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle StolenVehicle { get; set; }
        public string ReportingOfficerId { get; set; }
        public Officer ReportedByOfficer { get; set; }
        public Guid ReportingPersonId { get; set; }
        public Person ReportedByPerson { get; set; }
        public string ResolvingOfficerId { get; set; }
        public Officer ResolvedByOfficer { get; set; }
        public string OfficialId { get; set; }
        public Official ProcessedByOfficial { get; set; }
    }
}
