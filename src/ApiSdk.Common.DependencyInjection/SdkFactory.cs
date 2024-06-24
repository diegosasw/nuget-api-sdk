using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace ApiSdk.Common.DependencyInjection;

public class SdkFactory
{
    private readonly HttpClient _httpClient;
    private readonly SdkAccessTokenProvider _authProvider;

    public SdkFactory(HttpClient httpClient, SdkAccessTokenProvider authProvider)
    {
        _httpClient = httpClient;
        _authProvider = authProvider;
    }
    
    /// <summary>
    /// Creates a new instance of <see cref="TClient"/>, used as a factory for the client.
    /// It uses reflection and expects the api client to accept a <see cref="HttpClientRequestAdapter"/> in the constructor.
    /// </summary>
    /// <returns>an instance of <see cref="TClient"/></returns>
    public TClient Create<TClient>()
        where TClient : BaseRequestBuilder
    {
        var httpClientRequestAdapter = 
            new HttpClientRequestAdapter(new SdkAuthenticationProvider(_authProvider), httpClient: _httpClient);
        
        return (TClient)Activator.CreateInstance(typeof(TClient), httpClientRequestAdapter)!;
    }
}