using System;

namespace IdentitySystem.Contracts.DTOs
{
    public class RefreshTokenDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expire { get; set; }
    }
}