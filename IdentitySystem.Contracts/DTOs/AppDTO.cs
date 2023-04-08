using System;

namespace IdentitySystem.Contracts.DTOs
{
    public class AppDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ClientSecret { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
        public DateTime LastEdit { get; set; }
        public string LastEditUser { get; set; }
    }
}

