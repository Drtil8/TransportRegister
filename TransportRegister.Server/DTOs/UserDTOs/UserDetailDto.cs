namespace TransportRegister.Server.DTOs.UserDTOs
{
    public class UserDetailDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsValid { get; set; }
        public int? PersonalId { get; set; }
        public string Rank{ get; set;}
    }
}
