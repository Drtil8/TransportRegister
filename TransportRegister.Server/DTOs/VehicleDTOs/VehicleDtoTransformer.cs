using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.VehicleDTOs
{
    public abstract class VehicleDtoTransformer
    {
        public static VehicleDetailDto TransformToDto(Vehicle vehicle)
        {
            if (vehicle == null)
                return null;

            VehicleDetailDto dto = vehicle switch
            {
                Car car => new CarDto
                {
                    NumberOfDoors = car.NumberOfDoors
                },
                Motorcycle motorcycle => new MotorcycleDto
                {
                    Constraints = motorcycle.Constraints
                },
                Bus bus => new BusDto
                {
                    SeatCapacity = bus.SeatCapacity,
                    StandingCapacity = bus.StandingCapacity
                },
                Truck => new TruckDto(),
                _ => null
            };

            if (dto != null)
            {
                dto.VehicleId = vehicle.VehicleId;
                dto.VehicleType = vehicle.GetType().Name;
                dto.VIN = vehicle.VIN;
                dto.Manufacturer = vehicle.Manufacturer;
                dto.Model = vehicle.Model;
                dto.HorsepowerKW = vehicle.Horsepower_KW;
                dto.EngineVolumeCM3 = vehicle.EngineVolume_CM3;
                dto.ManufacturedYear = vehicle.ManufacturedYear;
                dto.Color = vehicle.Color;
                dto.LengthCM = vehicle.Length_CM;
                dto.WidthCM = vehicle.Width_CM;
                dto.HeightCM = vehicle.Height_CM;
                dto.LoadCapacityKG = vehicle.LoadCapacity_KG;
                dto.ImageBase64 = vehicle.Image != null ? Convert.ToBase64String(vehicle.Image) : null;
                dto.CurrentLicensePlate = vehicle.LicensePlates?.FirstOrDefault()?.LicensePlate;
            }
            return dto;
        }

        public static Vehicle TransformToEntity(VehicleDetailDto dto)
        {
            if (dto == null)
                return null;

            Vehicle vehicle = dto switch
            {
                CarDto carDto => new Car
                {
                    NumberOfDoors = carDto.NumberOfDoors
                },
                MotorcycleDto motorcycleDto => new Motorcycle
                {
                    Constraints = motorcycleDto.Constraints
                },
                BusDto busDto => new Bus
                {
                    SeatCapacity = busDto.SeatCapacity,
                    StandingCapacity = busDto.StandingCapacity
                },
                TruckDto => new Truck(),
                _ => null
            };

            if (vehicle != null)
            {
                vehicle.VehicleId = dto.VehicleId;
                vehicle.VIN = dto.VIN;
                vehicle.Manufacturer = dto.Manufacturer;
                vehicle.Model = dto.Model;
                vehicle.Horsepower_KW = dto.HorsepowerKW;
                vehicle.EngineVolume_CM3 = dto.EngineVolumeCM3;
                vehicle.ManufacturedYear = dto.ManufacturedYear;
                vehicle.Color = dto.Color;
                vehicle.Length_CM = dto.LengthCM;
                vehicle.Width_CM = dto.WidthCM;
                vehicle.Height_CM = dto.HeightCM;
                vehicle.LoadCapacity_KG = dto.LoadCapacityKG;
                vehicle.Image = !string.IsNullOrEmpty(dto.ImageBase64) ? Convert.FromBase64String(dto.ImageBase64) : null;
                vehicle.OwnerId = dto.OwnerId;
                vehicle.OfficialId = dto.OfficialId;
            }
            return vehicle;
        }
    }
}
