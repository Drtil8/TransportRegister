#nullable enable
namespace TransportRegister.Server.Models

{
    public class Offence
    {
        public Guid OffenceId { get; set; } = Guid.NewGuid();
        public DateTime ReportedOn { get; set; }
        public bool IsValid { get; set; }

        public Guid? FineId { get; set; }
        public Fine? Fine { get; set; }

        public Guid VehicleId { get; set; }
        public Vehicle OffenceOnVehicle { get; set; }
        public string OfficerId { get; set; }
        public Officer ReportedByOfficer { get; set; }
        public string OfficialId { get; set; }
        public Official ProcessedByOfficial { get; set; }
        public Guid PersonId { get; set; }
        public Person CommitedBy {  get; set; }
    }
}
