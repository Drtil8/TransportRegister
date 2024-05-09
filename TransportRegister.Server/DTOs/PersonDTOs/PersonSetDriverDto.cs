namespace TransportRegister.Server.DTOs.PersonDTOs
{
    public class PersonSetDriverDto
    {
        public string DriversLicenseNumber { get; set; }
        public IEnumerable<string> Licenses { get; set; }
    }
}
