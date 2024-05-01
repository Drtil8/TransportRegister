using TransportRegister.Server.Data;
using TransportRegister.Server.Models;

namespace TransportRegister.Server.Seeds
{
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
                    Color = "Èerná metalíza",
                    Length_CM = 437.0,
                    Width_CM = 179.0,
                    Height_CM = 143.0,
                    OwnerId = 6,
                    NumberOfDoors = 5,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "BMW987654B",
                    Manufacturer = "BMW",
                    Model = "3 Series",
                    Horsepower_KW = 120.0,
                    EngineVolume_CM3 = 2500.0,
                    ManufacturedYear = 2019,
                    Color = "Støíbrná",
                    Length_CM = 456.0,
                    Width_CM = 185.0,
                    Height_CM = 150.0,
                    OwnerId = 7,
                    NumberOfDoors = 4,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "MAZ123456M",
                    Manufacturer = "Mazda",
                    Model = "Miata",
                    Horsepower_KW = 110.0,
                    EngineVolume_CM3 = 2200.0,
                    ManufacturedYear = 2018,
                    Color = "Støíbrná",
                    Length_CM = 470.0,
                    Width_CM = 180.0,
                    Height_CM = 145.0,
                    OwnerId = 8,
                    NumberOfDoors = 4,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/mazda_miata.jpg")
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
                    Color = "Èerná",
                    Length_CM = 208.0,
                    Width_CM = 80.0,
                    Height_CM = 106.0,
                    OwnerId = 9,
                    Constraints = "<50cm3",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "YAM987654Y",
                    Manufacturer = "Yamaha",
                    Model = "YZF-R6",
                    Horsepower_KW = 90.0,
                    EngineVolume_CM3 = 600.0,
                    ManufacturedYear = 2022,
                    Color = "Modrá",
                    Length_CM = 205.0,
                    Width_CM = 70.0,
                    Height_CM = 110.0,
                    OwnerId = 10,
                    Constraints = "<100cm3>",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "KAW123456K",
                    Manufacturer = "Kawasaki",
                    Model = "Ninja 400",
                    Horsepower_KW = 45.0,
                    EngineVolume_CM3 = 399.0,
                    ManufacturedYear = 2021,
                    Color = "Zelená",
                    Length_CM = 200.0,
                    Width_CM = 75.0,
                    Height_CM = 110.0,
                    OwnerId = 11,
                    Constraints = "Žádné",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
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
                    Color = "Bílá",
                    Length_CM = 800.0,
                    Width_CM = 200.0,
                    Height_CM = 300.0,
                    OwnerId = 6,
                    SeatCapacity = 70,
                    StandingCapacity = 100,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "MAN123456M",
                    Manufacturer = "MAN",
                    Model = "Lion's Coach",
                    Horsepower_KW = 390.0,
                    EngineVolume_CM3 = 10500.0,
                    ManufacturedYear = 2020,
                    Color = "Fialová",
                    Length_CM = 900.0,
                    Width_CM = 250.0,
                    Height_CM = 350.0,
                    OwnerId = 7,
                    SeatCapacity = 60,
                    StandingCapacity = 80,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "VOL123456V",
                    Manufacturer = "Volvo",
                    Model = "7900 Electric",
                    Horsepower_KW = 250.0,
                    EngineVolume_CM3 = 0.0,
                    ManufacturedYear = 2022,
                    Color = "Modrá",
                    Length_CM = 1200.0,
                    Width_CM = 250.0,
                    Height_CM = 350.0,
                    OwnerId = 8,
                    SeatCapacity = 45,
                    StandingCapacity = 60,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
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
                    OwnerId = 9,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "DAF123456D",
                    Manufacturer = "DAF",
                    Model = "XF",
                    Horsepower_KW = 380.0,
                    EngineVolume_CM3 = 12000.0,
                    ManufacturedYear = 2019,
                    Color = "Žlutá",
                    Length_CM = 600.0,
                    Width_CM = 220.0,
                    Height_CM = 280.0,
                    OwnerId = 10,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
                },

                new()
                {
                    VIN = "MAN123456M",
                    Manufacturer = "MAN",
                    Model = "500E",
                    Horsepower_KW = 390.0,
                    EngineVolume_CM3 = 13000.0,
                    ManufacturedYear = 2010,
                    Color = "Èerná",
                    Length_CM = 800.0,
                    Width_CM = 320.0,
                    Height_CM = 290.0,
                    OwnerId = 11,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = File.ReadAllBytes("Seeds/Images/Cars/car_generic.png")
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
}
