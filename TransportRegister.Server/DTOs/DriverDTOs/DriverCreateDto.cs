using TransportRegister.Server.Models;

namespace TransportRegister.Server.DTOs.DriverDTOs
{
    public class DriverCreateDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthNumber { get; set; }
        public bool Sex_Male { get; set; }
        public DateOnly DateOfBirth { get; set; }
        //public Address Address { get; set; }
        //public byte?[] Image { get; set; }

        public string OfficialId { get; set; }
    }
}
