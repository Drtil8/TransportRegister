namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class PersonSimpleListDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthNumber { get; set; }
    }

    public class DriverSimpleListDto : PersonSimpleListDto
    {
        public string DriversLicenseNumber { get; set; }
    }
}
