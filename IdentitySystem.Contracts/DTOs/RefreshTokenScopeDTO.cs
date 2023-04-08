namespace IdentitySystem.Contracts.DTOs
{
    public class RefreshTokenScopeDTO
    {
        public int Id { get; set; }
        public int RefreshTokenId { get; set; }
        public string Scope { get; set; }
    }
}