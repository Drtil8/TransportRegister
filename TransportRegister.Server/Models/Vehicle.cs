namespace TransportRegister.Server.Models;

public abstract class Vehicle
{
    public Guid VehicleId { get; set; } = Guid.NewGuid();
    public string VIN { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public string Horsepower_KW { get; set; }
    public int EngineVolume_CM3 { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Length_CM { get; set; }
    public int Width_CM3 { get; set; }
    public int LoadCapacity {  get; set; }

    public ICollection<LicensePlateHistory> LicensePlates { get; set; }
    public ICollection<Offence> Offences { get; set; }
    public ICollection<Theft> Thefts { get; set; }

    public Guid OwnerId { get; set; }
    public Owner Owner { get; set; }

    public string OfficialId { get; set; }
    public Official AddedByOfficial { get; set; }
}

public class Car : Vehicle
{

}

public class Truck : Vehicle
{

}

public class Motorcycle : Vehicle
{
    public string Constraints { get; set; }
}

public class Bus : Vehicle
{
    public int SeatCapacity { get; set; }
    public int StandingCapacity { get; set; }
}
