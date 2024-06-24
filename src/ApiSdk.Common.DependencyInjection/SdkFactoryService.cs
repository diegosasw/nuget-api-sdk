using ApiSdk.Common.Factories;
using ApiSdk.Common.Options;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
// ReSharper disable ConvertToPrimaryConstructor

namespace ApiSdk.Common.DependencyInjection;

public class SdkFactoryService<TOptions>
    where TOptions : BaseSdkAuthenticationOptions
{
    private readonly HttpClient _httpClient;
    private readonly SdkAccessTokenProvider<TOptions> _authProvider;

    public SdkFactoryService(HttpClient httpClient, SdkAccessTokenProvider<TOptions> authProvider)
    {
        _httpClient = httpClient;
        _authProvider = authProvider;
    }
    
    /// <summary>
    /// Creates a new instance of <see cref="TClient"/>, used as a factory for the client.
    /// </summary>
    /// <returns>an instance of <see cref="TClient"/></returns>
    public TClient Create<TClient>()
        where TClient : BaseRequestBuilder
    {
        var httpClientRequestAdapter = 
            new HttpClientRequestAdapter(new SdkAuthenticationProvider(_authProvider), httpClient: _httpClient);

        return ApiClientFactory.Create<TClient>(httpClientRequestAdapter);
    }
}