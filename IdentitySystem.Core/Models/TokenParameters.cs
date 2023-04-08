namespace IdentitySystem.Core.Models;

public class TokenParameters
{
    public string GrantType { get; set; }
    public string Code { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}
