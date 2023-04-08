namespace IdentitySystem.Contracts.DTOs
{
    public class AuthorizationScopeDTO
    {
        public int Id { get; set; }
        public int AuthorizationId { get; set; }
        public string Scope { get; set; }
    }
}