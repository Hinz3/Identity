using System;

namespace IdentitySystem.Contracts.DTOs
{
    public class AppUrlDTO
    {
        public int Id { get; set; }
        public Guid AppId { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
        public DateTime LastEdit { get; set; }
        public string LastEditUser { get; set; }
    }
}
