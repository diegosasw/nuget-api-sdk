// See https://aka.ms/new-console-template for more information

using ApiSdk.Common.Builders;
using Microsoft.Extensions.Configuration;
using WebApiOne.Client.Options;
using WebApiOne.Client.Sdk;

var builder = new ConfigurationBuilder();

var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";

builder
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Build();

// Read settings
var webApiOneAuthOptions = new WebApiOneAuthOptions();
config.Bind("SdkWebApiOneSample", webApiOneAuthOptions);

try
{
    var webApiOneClient = 
        new ApiClientBuilder<WebApiOneClient, WebApiOneAuthOptions>(webApiOneAuthOptions, "https://vitesse.free.beeceptor.com")
            .Build();
    var projects = await webApiOneClient.Projects.GetAsync();
    foreach (var project in projects!)
    {
        Console.WriteLine(project.Name);
    }
}

catch (Exception exception)
{
    Console.Error.WriteLine(exception);
}
