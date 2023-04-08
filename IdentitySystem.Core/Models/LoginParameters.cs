namespace IdentitySystem.Core.Models;

public class LoginParameters
{
    public string ResponseCode { get; set; }
    public string ClientId { get; set; }
    public string RedirectUrl { get; set; }
    public List<string> Scopes { get; set; }
    public string State { get; set; }
}
