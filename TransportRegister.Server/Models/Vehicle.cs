namespace TransportRegister.Server.Models;

public class Vehicle
{
    public Guid VehicleId { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
}