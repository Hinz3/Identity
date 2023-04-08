using System;

namespace IdentitySystem.Contracts.DTOs
{
    public class ScopeAudienceDTO
    {
        public int Id { get; set; }
        public int ScopeId { get; set; }
        public int AudienceId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedUser { get; set; }
    }
}
