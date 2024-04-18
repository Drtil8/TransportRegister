namespace TransportRegister.Server.DTOs.VehicleDTOs
{
    public abstract class VehicleDto
    {
        public int VehicleId { get; set; }
        public string VIN { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double HorsepowerKW { get; set; }
        public double EngineVolumeCM3 { get; set; }
        public int ManufacturedYear { get; set; }
        public string Color { get; set; }
        public double LengthCM { get; set; }
        public double WidthCM { get; set; }
        public double HeightCM { get; set; }
        public double LoadCapacityKG { get; set; }
        public int OwnerId { get; set; }
        public string OfficialId { get; set; }
        public string VehicleType { get; set; }
        
        // TODO maybe add list of Thefts and Fines (need TheftDto and FineDto)
    }
    
    public class TruckDto : VehicleDto
    {
        
    }
    
    public class MotorcycleDto : VehicleDto
    {
        public string Constraints { get; set; }
    }
    
    public class CarDto : VehicleDto
    {
        public int NumberOfDoors { get; set; }
    }
    
    public class BusDto : VehicleDto
    {
        public int SeatCapacity { get; set; }
        public int StandingCapacity { get; set; }
    }
}
