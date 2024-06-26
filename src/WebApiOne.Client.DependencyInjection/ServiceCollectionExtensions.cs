using ApiSdk.Common;
using ApiSdk.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiOne.Client.Options;
using WebApiOne.Client.Sdk;

namespace WebApiOne.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiOneClient(
        this IServiceCollection services,
        string authOptionsSectionName = Constants.Settings.AuthenticationSectionName,
        string apiBaseUrl = Constants.Api.BaseUrl)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(authOptionsSectionName);

        services.AddKiotaHandlers();
        services.AddMemoryCache();
        
        services
            .AddOptions<WebApiOneAuthOptions>()
            .BindConfiguration(authOptionsSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddHttpClient<SdkFactoryService<WebApiOneAuthOptions>>((_, client) =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            })
            .AddKiotaHandlers();

        services.AddHttpClient<SdkAccessTokenProvider<WebApiOneAuthOptions>>();
        
        services.TryAddTransient<WebApiOneClient>(sp =>
        {
            var factory = sp.GetRequiredService<SdkFactoryService<WebApiOneAuthOptions>>();
            return factory.Create<WebApiOneClient>();
        });

        return services;
    }
}