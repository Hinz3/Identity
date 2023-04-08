using System.Text.Json.Serialization;

namespace DemoWebApp.Models.Requests;

public class TokenRequest
{
    [JsonPropertyName("grantType")]
    public string GrantType { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    public string ClientSecret { get; set; }

}
