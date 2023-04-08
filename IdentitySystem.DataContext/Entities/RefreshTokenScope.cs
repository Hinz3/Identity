namespace IdentitySystem.DataContext.Entities;

public class RefreshTokenScope
{
    public int Id { get; set; }
    public int RefreshTokenId { get; set; }
    public string Scope { get; set; }
}
