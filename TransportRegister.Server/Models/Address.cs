using Microsoft.EntityFrameworkCore;

namespace TransportRegister.Server.Models
{
    [Owned] // TODO: skusiť
    public class Address
    {
        public string Street { get; set; }    // TODO: tabulka adresy -- krajina, kraj, ulica, mesto, čislo domu, psč -- otestovať
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int HouseNumber { get; set; }
        public int PostalCode { get; set; }
    }
}
