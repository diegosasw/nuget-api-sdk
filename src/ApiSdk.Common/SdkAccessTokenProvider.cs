using System.Net.Http.Json;
using ApiSdk.Common.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions.Authentication;

namespace ApiSdk.Common;

public class SdkAccessTokenProvider<TOptions>
    : IAccessTokenProvider
    where TOptions : BaseSdkAuthenticationOptions
{
    private readonly BaseSdkAuthenticationOptions _sdkAuthenticationOptions;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache? _memoryCache;

    // ReSharper disable once ConvertToPrimaryConstructor
    public SdkAccessTokenProvider(
        IOptions<TOptions> sdkAuthenticationOptions,
        HttpClient httpClient,
        IMemoryCache? memoryCache)
    {
        _sdkAuthenticationOptions = sdkAuthenticationOptions.Value;
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }
    
    /// <inheritdoc/>
    public Task<string> GetAuthorizationTokenAsync(
        Uri uri, 
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = new())
    {
        if (!AllowedHostsValidator.IsUrlHostValid(uri))
        {
            throw new InvalidOperationException($"The provided Url {uri} is not allowed");
        }

        return GetAccessTokenAsync(
            _sdkAuthenticationOptions.TokenEndpointUrl,
            _sdkAuthenticationOptions.ClientId,
            _sdkAuthenticationOptions.Secret,
            _sdkAuthenticationOptions.TokenCacheKey,
            cancellationToken);
    }

    /// <inheritdoc/>
    public AllowedHostsValidator AllowedHostsValidator => new([]);
    
    private async Task<string> GetAccessTokenAsync(
        string tokenEndpointUrl,
        string clientId, 
        string secret, 
        string tokenCacheKey,
        CancellationToken cancellationToken)
    {
        var isCacheEnabled = _memoryCache is not null && !string.IsNullOrWhiteSpace(tokenCacheKey);
        if (isCacheEnabled && _memoryCache?.TryGetValue(tokenCacheKey, out string? token) == true)
        {
            return token!;
        }
        
        var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpointUrl);
        //request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}")));
        request.Content =
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = clientId,
                ["client_secret"] = secret
            });
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        try
        {
            var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResult>(cancellationToken);
            if (isCacheEnabled)
            {
                _memoryCache?.Set(tokenCacheKey, tokenResponse!.AccessToken, TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 300));
            }
            return tokenResponse!.AccessToken;
        }
        catch (Exception exception)
        {
            throw new TokenProviderException($"Unable to retrieve valid token from {tokenEndpointUrl}", exception);
        }
    }
}