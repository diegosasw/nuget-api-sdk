// See https://aka.ms/new-console-template for more information

using ApiSdk.Common;
using ApiSdk.Common.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WebApiOne.Client.Sdk;

var builder = new ConfigurationBuilder();
builder
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Build();

// Create IOptions<SdkOptions> instance
var sdkOptions = new SdkOptions();
config.GetSection("SdkWebApiOneSample").Bind(sdkOptions);
var options = Options.Create(sdkOptions);

// Create HttpClient instance
var httpClient = new HttpClient() {BaseAddress = new Uri(sdkOptions.TokenEndpointUrl) };

// Create optional IMemoryCache instance
var memoryCacheOptions = new MemoryCacheOptions();
var memoryCache = new MemoryCache(memoryCacheOptions);

try
{
    var tokenProvider = new SdkAccessTokenProvider(options, httpClient, memoryCache);
    var authenticationProvider = new SdkAuthenticationProvider(tokenProvider);
    var adapter =
        new HttpClientRequestAdapter(authenticationProvider)
        {
            BaseUrl = sdkOptions.BaseUrl
        };
    
    var client = new WebApiOneClient(adapter);
    var projects = await client.Projects.GetAsync();
    foreach (var project in projects!)
    {
        Console.WriteLine(project);
    }
}

catch (Exception exception)
{
    Console.Error.WriteLine(exception);
}
