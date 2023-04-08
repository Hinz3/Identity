using System;
using System.Collections.Generic;

namespace IdentitySystem.Contracts.DTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public DateTime Expires { get; set; }
        public string RefreshToken { get; set; }
        public List<string> Scopes { get; set; }
    }
}
