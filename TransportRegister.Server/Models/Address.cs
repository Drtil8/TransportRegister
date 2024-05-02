using Microsoft.EntityFrameworkCore;

namespace TransportRegister.Server.Models
{
    [Owned]
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
    }
}
