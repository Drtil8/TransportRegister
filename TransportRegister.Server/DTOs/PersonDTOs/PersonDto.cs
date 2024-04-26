using System.ComponentModel.DataAnnotations;
using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class AddressDto
    {
        public string Street { get; set; }    // TODO: tabulka adresy -- krajina, kraj, ulica, mesto, čislo domu, psč -- otestovať
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
    }
    public class PersonDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthNumber { get; set; }
        public bool Sex_Male { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public AddressDto AddressDto { get; set; }

        public string OfficialId { get; set; }
        //public ICollection<Offence> CommitedOffences { get; set; }  // TODO: Needs Offence and thefts Dto's
        //public ICollection<Theft> ReportedThefts { get; set; }

    }

    public class DriverDto : PersonDto
    {
        public string DriversLicenseNumber { get; set; }
        public int BadPoints { get; set; }
        public bool HasSuspendedLicense { get; set; }
        public DateTime? LastCrimeCommited { get; set; }
        public DateTime? DrivingSuspendedUntil { get; set; }

        public IEnumerable<DriversLicenseDto> Licenses { get; set; } // Licenses DTo
    }
    
    public class OwnerDto : PersonDto
    {
        public IEnumerable<VehicleDetailDto> Vehicles { get; set; }
    }
}

