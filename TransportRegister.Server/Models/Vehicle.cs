namespace TransportRegister.Server.Models;

public abstract class Vehicle
{
    public int VehicleId { get; set; }
    public string VIN { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public double Horsepower_KW { get; set; }
    public double EngineVolume_CM3 { get; set; }
    public int ManufacturedYear { get; set; }
    public string Color { get; set; }
    public double Length_CM { get; set; }
    public double Width_CM { get; set; }
    public double Height_CM { get; set; }
    public double LoadCapacity_KG { get; set; }
    public byte[] Image { get; set; } = default!;
    public int OwnerId { get; set; }            // TODO: owner == �t�t (komunizmus !!)
    public Owner Owner { get; set; }
    public string OfficialId { get; set; }
    public Official AddedByOfficial { get; set; }
    public ICollection<LicensePlateHistory> LicensePlates { get; set; }
    public ICollection<Offence> Offences { get; set; }
    public ICollection<Theft> Thefts { get; set; }
}

public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }
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
