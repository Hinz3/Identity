namespace IdentitySystem.Core.Models;

public class GrantTypeResponse
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public List<string> Scopes { get; set; }
}
