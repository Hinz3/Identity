namespace IdentitySystem.Core.Configurations;

public class JWTConfiguration
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    public bool RequireSignedTokens { get; set; }
    public bool RequireExpirationTime { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateIssuer { get; set; }
}
