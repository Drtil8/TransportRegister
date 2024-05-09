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
                    VIN = "1HGCM82633A004352",
                    Manufacturer = "Toyota",
                    Model = "Corolla",
                    Horsepower_KW = 100.0,
                    EngineVolume_CM3 = 2000.0,
                    ManufacturedYear = 2020,
                    Color = "Černá metalýza",
                    Length_CM = 437.0,
                    Width_CM = 179.0,
                    Height_CM = 143.0,
                    OwnerId = 6,
                    NumberOfDoors = 5,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = null
                },

                new()
                {
                    VIN = "1HGBH41JXMN109186",
                    Manufacturer = "BMW",
                    Model = "3 Series",
                    Horsepower_KW = 120.0,
                    EngineVolume_CM3 = 2500.0,
                    ManufacturedYear = 2019,
                    Color = "Stříbrná",
                    Length_CM = 456.0,
                    Width_CM = 185.0,
                    Height_CM = 150.0,
                    OwnerId = 7,
                    NumberOfDoors = 4,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = null
                },

                new()
                {
                    VIN = "1J4FY29P2VPB58794",
                    Manufacturer = "Mazda",
                    Model = "Miata",
                    Horsepower_KW = 110.0,
                    EngineVolume_CM3 = 2200.0,
                    ManufacturedYear = 2018,
                    Color = "Stříbrná",
                    Length_CM = 470.0,
                    Width_CM = 180.0,
                    Height_CM = 145.0,
                    OwnerId = 8,
                    NumberOfDoors = 4,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = null
                }
            };

            var motorcyclesToSeed = new Motorcycle[]
            {
                new()
                {
                    VIN = "2FMDK3GC4BBA83279",
                    Manufacturer = "Honda",
                    Model = "cb500f",
                    Horsepower_KW = 35.0,
                    EngineVolume_CM3 = 471.0,
                    ManufacturedYear = 2023,
                    Color = "ČernÁ",
                    Length_CM = 208.0,
                    Width_CM = 80.0,
                    Height_CM = 106.0,
                    OwnerId = 9,
                    Constraints = "<50cm3",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = null
                },

                new()
                {
                    VIN = "3D7TT2CT5AG134508",
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
                    Image = null
                },

                new()
                {
                    VIN = "3FAHP0HA2CR267305",
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
                    Constraints = "žádné",
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = null
                }
            };

            var busesToSeed = new Bus[]
            {
                new()
                {
                    VIN = "4T1BK36B48U276831",
                    Manufacturer = "SOR",
                    Model = "C 12",
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
                    Image = null
                },

                new()
                {
                    VIN = "5GZCZ43D13S812715",
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
                    Image = null
                },

                new()
                {
                    VIN = "5XYKT3A16BG160987",
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
                    Image = null
                }
            };

            var trucksToSeed = new Truck[]
            {
                new()
                {
                    VIN = "6F4XK52G94Z173451",
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
                    Image = null
                },

                new()
                {
                    VIN = "6G2VX12G74L237914",
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
                    Image = null
                },

                new()
                {
                    VIN = "7FARW2H58JE057642",
                    Manufacturer = "MAN",
                    Model = "500E",
                    Horsepower_KW = 390.0,
                    EngineVolume_CM3 = 13000.0,
                    ManufacturedYear = 2010,
                    Color = "Černá",
                    Length_CM = 800.0,
                    Width_CM = 320.0,
                    Height_CM = 290.0,
                    OwnerId = 11,
                    OfficialId = "d6f46418-2c21-43f8-b167-162fb5e3a999",
                    Image = null
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
