using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TransportRegister.Server.Models
{
    public class User : IdentityUser
    {
        // public Guid UserId { get; set; } = Guid.NewGuid();
        public bool IsActive { get; set; } 
        public bool IsValid { get; set; }
        public bool IsAdmin { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
    }

    public class Official : User
    {
        public ICollection<Offence> ProcessedOffences { get; set; }
        public ICollection<Theft> ProcessedThefts { get; set; }
        public ICollection<Vehicle> AddedVehicles { get; set; }
        public ICollection<Person> AddedPersons { get; set; }


    }

    public class Officer : User
    {
        public int PersonalId { get; set; }
        public string Rank { get; set; }

        public ICollection<Offence> ReportedOffences { get; set; }
        public ICollection<Theft> ReportedThefts { get; set; }
        public ICollection<Theft> ResolvedThefts { get; set; }
    }
}
