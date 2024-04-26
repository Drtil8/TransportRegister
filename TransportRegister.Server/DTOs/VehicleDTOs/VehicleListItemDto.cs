namespace TransportRegister.Server.DTOs.VehicleDTOs
{
    public class VehicleListItemDto
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }
        public string VehicleType { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int ManufacturedYear { get; set; }
        public int OwnerId { get; set; }
        public string OwnerFullName { get; set; }
    }
}
