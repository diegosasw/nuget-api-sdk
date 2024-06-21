// See https://aka.ms/new-console-template for more information

using ApiSdk.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using WebApiOne.Client.Sdk;

var builder = new ConfigurationBuilder();
builder
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Build();

var sdkSettings = new SdkOptions();
config.Bind("Sdk", sdkSettings);

try
{
    var provider = new AnonymousAuthenticationProvider();
    var adapter = new HttpClientRequestAdapter(provider)
    {
        BaseUrl = sdkSettings.BaseUrl
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
