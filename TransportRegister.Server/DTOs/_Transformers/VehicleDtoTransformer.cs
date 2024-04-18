using TransportRegister.Server.DTOs.VehicleDTOs;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs._Transformers
{
    public abstract class VehicleDtoTransformer
    {
        public static VehicleDto TransformToDto(Vehicle vehicle)
        {
            return vehicle switch
            {
                Car car => new CarDto
                {
                    VehicleId = car.VehicleId,
                    VIN = car.VIN,
                    Manufacturer = car.Manufacturer,
                    Model = car.Model,
                    HorsepowerKW = car.Horsepower_KW,
                    EngineVolumeCM3 = car.EngineVolume_CM3,
                    ManufacturedYear = car.ManufacturedYear,
                    Color = car.Color,
                    LengthCM = car.Length_CM,
                    WidthCM = car.Width_CM,
                    HeightCM = car.Height_CM,
                    LoadCapacityKG = car.LoadCapacity_KG,
                    VehicleType = "Car",
                    NumberOfDoors = car.NumberOfDoors
                },
                Motorcycle motorcycle => new MotorcycleDto
                {
                    VehicleId = motorcycle.VehicleId,
                    VIN = motorcycle.VIN,
                    Manufacturer = motorcycle.Manufacturer,
                    Model = motorcycle.Model,
                    HorsepowerKW = motorcycle.Horsepower_KW,
                    EngineVolumeCM3 = motorcycle.EngineVolume_CM3,
                    ManufacturedYear = motorcycle.ManufacturedYear,
                    Color = motorcycle.Color,
                    LengthCM = motorcycle.Length_CM,
                    WidthCM = motorcycle.Width_CM,
                    HeightCM = motorcycle.Height_CM,
                    LoadCapacityKG = motorcycle.LoadCapacity_KG,
                    VehicleType = "Motorcycle",
                    Constraints = motorcycle.Constraints
                },
                Bus bus => new BusDto
                {
                    VehicleId = bus.VehicleId,
                    VIN = bus.VIN,
                    Manufacturer = bus.Manufacturer,
                    Model = bus.Model,
                    HorsepowerKW = bus.Horsepower_KW,
                    EngineVolumeCM3 = bus.EngineVolume_CM3,
                    ManufacturedYear = bus.ManufacturedYear,
                    Color = bus.Color,
                    LengthCM = bus.Length_CM,
                    WidthCM = bus.Width_CM,
                    HeightCM = bus.Height_CM,
                    LoadCapacityKG = bus.LoadCapacity_KG,
                    VehicleType = "Bus",
                    SeatCapacity = bus.SeatCapacity,
                    StandingCapacity = bus.StandingCapacity
                },
                Truck bus => new TruckDto
                {
                    VehicleId = bus.VehicleId,
                    VIN = bus.VIN,
                    Manufacturer = bus.Manufacturer,
                    Model = bus.Model,
                    HorsepowerKW = bus.Horsepower_KW,
                    EngineVolumeCM3 = bus.EngineVolume_CM3,
                    ManufacturedYear = bus.ManufacturedYear,
                    Color = bus.Color,
                    LengthCM = bus.Length_CM,
                    WidthCM = bus.Width_CM,
                    HeightCM = bus.Height_CM,
                    LoadCapacityKG = bus.LoadCapacity_KG,
                    VehicleType = "Truck"
                },
                _ => null
            };
        }
        
        public static Vehicle TransformToEntity(VehicleDto dto)
        {
            return dto switch
            {
                CarDto carDto => new Car
                {
                    VehicleId = carDto.VehicleId,
                    VIN = carDto.VIN,
                    Manufacturer = carDto.Manufacturer,
                    Model = carDto.Model,
                    Horsepower_KW = carDto.HorsepowerKW,
                    EngineVolume_CM3 = carDto.EngineVolumeCM3,
                    ManufacturedYear = carDto.ManufacturedYear,
                    Color = carDto.Color,
                    Length_CM = carDto.LengthCM,
                    Width_CM = carDto.WidthCM,
                    Height_CM = carDto.HeightCM,
                    LoadCapacity_KG = carDto.LoadCapacityKG,
                    OwnerId = carDto.OwnerId,
                    OfficialId = carDto.OfficialId,
                    NumberOfDoors = carDto.NumberOfDoors
                },
                MotorcycleDto motorcycleDto => new Motorcycle
                {
                    VehicleId = motorcycleDto.VehicleId,
                    VIN = motorcycleDto.VIN,
                    Manufacturer = motorcycleDto.Manufacturer,
                    Model = motorcycleDto.Model,
                    Horsepower_KW = motorcycleDto.HorsepowerKW,
                    EngineVolume_CM3 = motorcycleDto.EngineVolumeCM3,
                    ManufacturedYear = motorcycleDto.ManufacturedYear,
                    Color = motorcycleDto.Color,
                    Length_CM = motorcycleDto.LengthCM,
                    Width_CM = motorcycleDto.WidthCM,
                    Height_CM = motorcycleDto.HeightCM,
                    LoadCapacity_KG = motorcycleDto.LoadCapacityKG,
                    OwnerId = motorcycleDto.OwnerId,
                    OfficialId = motorcycleDto.OfficialId,
                    Constraints = motorcycleDto.Constraints
                },
                BusDto busDto => new Bus
                {
                    VehicleId = busDto.VehicleId,
                    VIN = busDto.VIN,
                    Manufacturer = busDto.Manufacturer,
                    Model = busDto.Model,
                    Horsepower_KW = busDto.HorsepowerKW,
                    EngineVolume_CM3 = busDto.EngineVolumeCM3,
                    ManufacturedYear = busDto.ManufacturedYear,
                    Color = busDto.Color,
                    Length_CM = busDto.LengthCM,
                    Width_CM = busDto.WidthCM,
                    Height_CM = busDto.HeightCM,
                    LoadCapacity_KG = busDto.LoadCapacityKG,
                    OwnerId = busDto.OwnerId,
                    OfficialId = busDto.OfficialId,
                    SeatCapacity = busDto.SeatCapacity,
                    StandingCapacity = busDto.StandingCapacity
                },
                TruckDto truckDto => new Truck
                {
                    VehicleId = truckDto.VehicleId,
                    VIN = truckDto.VIN,
                    Manufacturer = truckDto.Manufacturer,
                    Model = truckDto.Model,
                    Horsepower_KW = truckDto.HorsepowerKW,
                    EngineVolume_CM3 = truckDto.EngineVolumeCM3,
                    ManufacturedYear = truckDto.ManufacturedYear,
                    Color = truckDto.Color,
                    Length_CM = truckDto.LengthCM,
                    Width_CM = truckDto.WidthCM,
                    Height_CM = truckDto.HeightCM,
                    LoadCapacity_KG = truckDto.LoadCapacityKG,
                    OwnerId = truckDto.OwnerId,
                    OfficialId = truckDto.OfficialId
                },
                _ => null
            };
        }
    }
}
