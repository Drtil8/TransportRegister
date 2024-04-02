namespace TransportRegister.Server.Models
{
    public class Owner
    {
        public Guid OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
