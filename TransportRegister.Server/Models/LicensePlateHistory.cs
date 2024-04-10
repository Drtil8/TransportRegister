using System.ComponentModel.DataAnnotations.Schema;

namespace TransportRegister.Server.Models
{
    public class LicensePlateHistory

    {
        public Guid LicensePlateHistoryId { get; set; } = Guid.NewGuid();
        public string LicensePlate { get; set; }
        public DateTime ChangedOn { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
