using ApiSdk.Common.Factories;
using ApiSdk.Common.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
// ReSharper disable ConvertToPrimaryConstructor

namespace ApiSdk.Common.Builders;

public class ApiClientBuilder<TClient, TAuthOptions>
    where TClient : BaseRequestBuilder
    where TAuthOptions : BaseSdkAuthenticationOptions
{
    private readonly TAuthOptions _authOptions;
    private readonly string _apiBaseUrl;
    private HttpClient _authHttpClient = new();
    private IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

    public ApiClientBuilder(TAuthOptions authOptions, string apiBaseUrl = Constants.Api.BaseUrl)
    {
        _authOptions = authOptions;
        _apiBaseUrl = apiBaseUrl;
    }
    
    public ApiClientBuilder<TClient, TAuthOptions> WithAuthHttpClient(HttpClient authHttpClient)
    {
        _authHttpClient = authHttpClient;
        return this;
    }
    
    public ApiClientBuilder<TClient, TAuthOptions> WithMemoryCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        return this;
    }
    
    public TClient Build()
    {
        _authHttpClient.BaseAddress ??= new Uri(_authOptions.TokenEndpointUrl);
        
        var tokenProvider = 
            new SdkAccessTokenProvider<TAuthOptions>(
                Microsoft.Extensions.Options.Options.Create(_authOptions), 
                _authHttpClient, 
                _memoryCache);
        
        var authenticationProvider = new SdkAuthenticationProvider(tokenProvider);
        var httpClientRequestAdapter =
            new HttpClientRequestAdapter(authenticationProvider)
            {
                BaseUrl = _apiBaseUrl
            };

        return ApiClientFactory.Create<TClient>(httpClientRequestAdapter);
    }
}