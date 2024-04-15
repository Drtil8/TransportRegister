using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds;

public class VehicleSeed
{
    public static void Seed(AppDbContext context)
    {
        var carsToSeed = new Car[]
        {
            new()
            {
                VIN = "TOY123456T",
                Manufacturer = "Toyota",
                Model = "Corolla",
                Horsepower_KW = 100.0,
                EngineVolume_CM3 = 2000.0,
                ManufacturedYear = 2020,
                Color = "Èierna metalíza",
                Length_CM = 437.0,
                Width_CM = 179.0,
                Height_CM = 143.0,
                OwnerId = 1,
                NumberOfDoors = 5,
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
            }
        };
        var motorcyclesToSeed = new Motorcycle[]
        {
            new()
            {
                VIN = "HON123456H",
                Manufacturer = "Honda",
                Model = "cb500f",
                Horsepower_KW = 35.0,
                EngineVolume_CM3 = 471.0,
                ManufacturedYear = 2023,
                Color = "Èierna",
                Length_CM = 208.0,
                Width_CM = 80.0,
                Height_CM = 106.0,
                OwnerId = 2,
                Constraints = "None",
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
            }
        };

        var busesToSeed = new Bus[]
        {
            new()
            {
                VIN = "SOR123456S",
                Manufacturer = "SOR",
                Model = "Normalny autobus",
                Horsepower_KW = 350.0,
                EngineVolume_CM3 = 4000.0,
                ManufacturedYear = 2023,
                Color = "Biela",
                Length_CM = 800.0,
                Width_CM = 200.0,
                Height_CM = 300.0,
                OwnerId = 2,
                SeatCapacity = 70,
                StandingCapacity = 100,
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
            }
        };

        var trucksToSeed = new Truck[]
        {
            new()
            {
                VIN = "AVI123456A",
                Manufacturer = "AVIA",
                Model = "100",
                Horsepower_KW = 300.0,
                EngineVolume_CM3 = 3000.0,
                ManufacturedYear = 1985,
                Color = "Modrá",
                Length_CM = 500.0,
                Width_CM = 180.0,
                Height_CM = 200.0,
                OwnerId = 2,
                OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999"
            }
        };

        foreach (var car in carsToSeed)
        {
            if (!context.Vehicles.Any(v => v.OwnerId == car.OwnerId))
            {
                context.Vehicles.Add(car);
            }
        }
        foreach (var motorcycle in motorcyclesToSeed)
        {
            if (!context.Vehicles.Any(v => v.OwnerId == motorcycle.OwnerId))
            {
                context.Vehicles.Add(motorcycle);
            }
        }
        foreach (var bus in busesToSeed)
        {
            if (!context.Vehicles.Any(v => v.OwnerId == bus.OwnerId))
            {
                context.Vehicles.Add(bus);
            }
        }
        foreach (var truck in trucksToSeed)
        {
            if (!context.Vehicles.Any(v => v.OwnerId == truck.OwnerId))
            {
                context.Vehicles.Add(truck);
            }
        }
        context.SaveChanges();
    }
}
