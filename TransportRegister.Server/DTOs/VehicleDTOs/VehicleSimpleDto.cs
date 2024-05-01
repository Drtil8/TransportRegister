using Microsoft.Build.ObjectModelRemoting;

namespace TransportRegister.Server.DTOs.VehicleDTOs
{
    public class VehicleSimpleDto
    {
        public int VehicleId { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
    }
}
