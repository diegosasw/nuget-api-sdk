using ApiSdk.Common;
using ApiSdk.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiTwo.Client.Options;
using WebApiTwo.Client.Sdk;

namespace WebApiTwo.Client.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebApiTwoClient(
        this IServiceCollection services,
        string authOptionsSectionName = Constants.Settings.AuthenticationSectionName,
        string apiBaseUrl = Constants.Api.BaseUrl)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(authOptionsSectionName);

        services.AddKiotaHandlers();
        services.AddMemoryCache();
        
        services
            .AddOptions<WebApiTwoAuthOptions>()
            .BindConfiguration(authOptionsSectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services
            .AddHttpClient<SdkFactoryService<WebApiTwoAuthOptions>>((_, client) =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            })
            .AddKiotaHandlers();

        services.AddHttpClient<SdkAccessTokenProvider<WebApiTwoAuthOptions>>();
        
        services.TryAddTransient<WebApiTwoClient>(sp =>
        {
            var factory = sp.GetRequiredService<SdkFactoryService<WebApiTwoAuthOptions>>();
            return factory.Create<WebApiTwoClient>();
        });

        return services;
    }
}