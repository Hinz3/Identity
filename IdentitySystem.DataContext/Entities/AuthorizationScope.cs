namespace IdentitySystem.DataContext.Entities;

public class AuthorizationScope
{
    public int Id { get; set; }
    public int AuthorizationId { get; set; }
    public string Scope { get; set; }
}
