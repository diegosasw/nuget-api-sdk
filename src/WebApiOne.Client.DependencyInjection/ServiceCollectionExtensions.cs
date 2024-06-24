using ApiSdk.Common;
using ApiSdk.Common.DependencyInjection;
using ApiSdk.Common.Options;
using WebApiOne.Client.DependencyInjection.Kiota;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApiOne.Client.Sdk;

namespace WebApiOne.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiOneClient(
        this IServiceCollection services,
        string configurationSectionName = "Sdk")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(configurationSectionName);

        services.AddKiotaHandlers();
        services.AddMemoryCache();
        
        services
            .AddOptions<SdkOptions>()
            .BindConfiguration(configurationSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddHttpClient<SdkFactory>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<SdkOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });

        services.AddHttpClient<SdkAccessTokenProvider>();
        
        services.AddTransient<WebApiOneClient>(sp =>
        {
            var factory = sp.GetRequiredService<SdkFactory>();
            return factory.Create<WebApiOneClient>();
        });

        return services;
    }
}