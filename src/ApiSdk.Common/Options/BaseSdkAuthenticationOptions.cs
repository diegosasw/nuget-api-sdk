namespace ApiSdk.Common.Options;

public abstract record BaseSdkAuthenticationOptions
{
    public string TokenEndpointUrl { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public string TokenCacheKey { get; init; } = string.Empty;
}