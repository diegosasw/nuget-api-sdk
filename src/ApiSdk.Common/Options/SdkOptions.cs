namespace ApiSdk.Common.Options;

public record SdkOptions
{
    public string BaseUrl { get; init; } = string.Empty;
    public IEnumerable<string> AllowedHosts { get; init; } = [];
    public string TokenEndpointUrl { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
}