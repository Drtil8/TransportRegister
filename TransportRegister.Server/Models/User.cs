﻿namespace TransportRegister.Server.Models
{
    public class User
    {
        public Guid UserId { get; set; }        // todo: Map to ClaimIdentity
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
