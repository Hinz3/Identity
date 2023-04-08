using System;

namespace RoleSystem.Contracts.DTOs
{
    public class RoleUserDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
    }
}