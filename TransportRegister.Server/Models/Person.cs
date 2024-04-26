using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TransportRegister.Server.Models
{
    // TODO: move to separate file
    //[Owned] // TODO: skusiť
    //public class Address
    //{
    //    public string Street { get; set; }    // TODO: tabulka adresy -- krajina, kraj, ulica, mesto, čislo domu, psč -- otestovať
    //    public string City { get; set; }
    //    public string State { get; set; }
    //    public string Country { get; set; }
    //    public int HouseNumber { get; set; }
    //    public int PostalCode { get; set; }
    //}

    public abstract class Person
    {
        public int PersonId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string BirthNumber { get; set; }
        public bool Sex_Male { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Address Address { get; set; }
        public byte?[] Image { get; set; }

        public string OfficialId { get; set; }
        public Official AddedByOfficial { get; set; }
        public ICollection<Offence> CommitedOffences { get; set; }  // TODO: zmazane new List
        public ICollection<Theft> ReportedThefts { get; set; }

    }

    public class Driver : Person
    {
        public string DriversLicenseNumber { get; set; }
        [Range(0, 12)]
        public int BadPoints { get; set; }
        public bool HasSuspendedLicense { get; set; }
        public DateTime? LastCrimeCommited { get; set; }        // TODO: is really nullable?
        public DateTime? DrivingSuspendedUntil { get; set; }

        public ICollection<DriversLicense> Licenses { get; set; }
    }

    public class Owner : Person
    {
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
