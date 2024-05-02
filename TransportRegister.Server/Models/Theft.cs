namespace TransportRegister.Server.Models
{
    public class Theft
    {
        public int TheftId { get; set; }
        public DateTime StolenOn { get; set; }
        public DateTime ReportedOn { get; set; }
        public DateTime? FoundOn { get; set; }
        public DateTime? ReturnedOn { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }

        public int VehicleId { get; set; }
        public Vehicle StolenVehicle { get; set; }
        public string ReportingOfficerId { get; set; }
        public Officer ReportedByOfficer { get; set; }
        public int ReportingPersonId { get; set; }
        public Person ReportedByPerson { get; set; }
        public string ResolvingOfficerId { get; set; }
        public Officer ResolvedByOfficer { get; set; }
        public string OfficialId { get; set; }
        public Official ProcessedByOfficial { get; set; }
    }
}
