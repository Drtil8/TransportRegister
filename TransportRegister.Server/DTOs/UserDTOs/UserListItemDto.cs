﻿namespace TransportRegister.Server.DTOs.UserDTOs
{
    public class UserListItemDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsValid { get; set; }
        public bool IsActive { get; set; }
    }
}
