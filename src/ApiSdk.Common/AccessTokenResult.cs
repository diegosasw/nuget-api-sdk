using System.Text.Json.Serialization;

namespace ApiSdk.Common;

public class AccessTokenResult
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; init; } = string.Empty;
    
    [JsonPropertyName("token_type")] 
    public required string TokenType { get; init; } = string.Empty;
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; } 
}