namespace TransportRegister.Server.DTOs.DriversLicenseDTOs
{
    public class DriversLicenseCreateDto
    { 
        public DateOnly IssuedOn { get; set; }
        public string Description { get; set; }
        public string VehicleType { get; set; }
    }
}
