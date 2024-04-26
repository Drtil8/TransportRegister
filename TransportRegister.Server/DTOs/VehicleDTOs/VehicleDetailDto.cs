using System.Text.Json.Serialization;

namespace TransportRegister.Server.DTOs.VehicleDTOs
{
    [JsonConverter(typeof(VehicleDtoConverter))]
    public abstract class VehicleDetailDto
    {
        public int VehicleId { get; set; }
        public string VIN { get; set; }
        public string LicensePlate { get; set; }        // todo add to VehicleDtoConverter
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
        public string VehicleType { get; set; }
        public int OwnerId { get; set; }
        public string OwnerFullName { get; set; }        // todo add to VehicleDtoConverter
        public string OfficialId { get; set; }
        public string OfficialFullName { get; set; }    // todo add to VehicleDtoConverter

        // TODO maybe add list of Thefts and Fines (need TheftDto and FineDto)
    }

    public class TruckDto : VehicleDetailDto
    {
    }

    public class MotorcycleDto : VehicleDetailDto
    {
        public string Constraints { get; set; }
    }

    public class CarDto : VehicleDetailDto
    {
        public int NumberOfDoors { get; set; }
    }

    public class BusDto : VehicleDetailDto
    {
        public int SeatCapacity { get; set; }
        public int StandingCapacity { get; set; }
    }
}
