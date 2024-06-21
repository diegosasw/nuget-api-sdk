using ApiSdk.Common.Options;
using WebApiOne.Client.DependencyInjection.Kiota;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApiOne.Client.Sdk;

namespace WebApiOne.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSdkClient(
        this IServiceCollection services,
        string configurationSectionName = "Sdk")
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(configurationSectionName);

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
            })
            .AddKiotaHandlers();

        services.AddHttpClient<SdkTokenProvider>();
        
        services.AddTransient<WebApiOneClient>(sp =>
        {
            var factory = sp.GetRequiredService<SdkFactory>();
            return factory.Create<WebApiOneClient>();
        });

        return services;
    }
}