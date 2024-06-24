using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace ApiSdk.Common.Factories;

public static class ApiClientFactory
{
    public static TClient Create<TClient>(HttpClientRequestAdapter httpClientRequestAdapter)
        where TClient : BaseRequestBuilder
    {
        ArgumentNullException.ThrowIfNull(httpClientRequestAdapter);
        return (TClient)Activator.CreateInstance(typeof(TClient), httpClientRequestAdapter)!;
    }
}