using System;

namespace RoleSystem.Contracts.DTOs
{
    public class RoleFunctionDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
    }
}