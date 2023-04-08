using System;

namespace IdentitySystem.Contracts.DTOs
{
    public class ScopeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
        public DateTime LastEdit { get; set; }
        public string LastEditUser { get; set; }
    }
}
