using Microsoft.AspNetCore.Identity;

namespace TransportRegister.Server.Models
{
    public class User : IdentityUser
    {
        public bool IsActive { get; set; }
    }

    public class Official : User
    {
        
    }

    public class Officer : User
    {
        public int PersonalId { get; set; }
        public string Rank { get; set; }
    }
}
