using System.ComponentModel.DataAnnotations;

namespace TransportRegister.Server.DTOs.UserDTOs
{
    /// <summary>
    /// DTO use for creating a new user
    /// </summary>
    /// <author> Dominik Pop </author>
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Role { get; set; }
        public string PersonalId { get; set; }
        public string Rank { get; set; }
    }
}
