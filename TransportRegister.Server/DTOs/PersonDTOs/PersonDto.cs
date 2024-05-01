using TransportRegister.Server.DTOs.DriversLicenseDTOs;
using TransportRegister.Server.DTOs.VehicleDTOs;
using System.Text.Json.Serialization;

namespace TransportRegister.Server.DTOs.PersonDTOs
{
    [JsonConverter(typeof(PersonDtoConverter))]
    public class PersonDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthNumber { get; set; }
        public bool Sex_Male { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public AddressDto AddressDto { get; set; }
        public string ImageBase64 { get; set; }
        public string PersonType { get; set; }

        public string OfficialId { get; set; }
        //public ICollection<Offence> CommitedOffences { get; set; }  // TODO: Needs Offence and thefts Dto's
        //public ICollection<Theft> ReportedThefts { get; set; }
        public IEnumerable<VehicleListItemDto> Vehicles { get; set; }
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
    }
}

