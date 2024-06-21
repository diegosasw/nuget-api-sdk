using System.Net.Http.Json;
using ApiSdk.Common;
using ApiSdk.Common.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Kiota.Abstractions.Authentication;

namespace WebApiOne.Client.DependencyInjection;

public class SdkTokenProvider
    : IAccessTokenProvider
{
    private readonly SdkOptions _sdkOptions;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache? _memoryCache;

    public SdkTokenProvider(
        SdkOptions sdkOptions,
        HttpClient httpClient,
        IMemoryCache? memoryCache)
    {
        _sdkOptions = sdkOptions;
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

        return GetAccessTokenAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public AllowedHostsValidator AllowedHostsValidator => new(_sdkOptions.AllowedHosts);
    
    private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        const string cacheKey = "SdkAccessToken";
        if (_memoryCache?.TryGetValue(cacheKey, out string? token) == true)
        {
            return token!;
        }

        var tokenEndpointUrl = _sdkOptions.TokenEndpointUrl;
        var clientId = _sdkOptions.ClientId;
        var secret = _sdkOptions.Secret;
        
        var request = new HttpRequestMessage(HttpMethod.Post, _sdkOptions.TokenEndpointUrl);
        //request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}")));
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = secret
        });
        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var tokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResult>(cancellationToken);
        _memoryCache?.Set(cacheKey, tokenResponse!.AccessToken, TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 300));
        return tokenResponse!.AccessToken;
    }
}