using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TransportRegister.Server.Models
{
    public abstract class Person
    {
        public Guid PersonId { get; set; } = Guid.NewGuid();

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string BirthNumber { get; set; }
        public string Address { get; set; }
        public int PostalCode { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool Sex_Male { get; set; }
        public string OfficialId { get; set; }
        public Official AddedByOfficial { get; set; }
        public ICollection<Offence> CommitedOffences { get; set; } = new List<Offence>();
        public ICollection<Theft> ReportedThefts { get; set; }

    }

    public class Driver : Person
    {
        public string DriversLicenseNumber { get; set; }
        public int BadPoints {  get; set; }
        public string Signature { get; set; }        // Not sure what type
        public bool HasSuspendedLicense { get; set; }
        public DateTime LastCrimeCommited { get; set; }
        public DateTime DrivingSuspendedUntil { get; set; }

        public ICollection<DriversLicense> Licenses {  get; set; }

    }

    public class Owner : Person
    {
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
