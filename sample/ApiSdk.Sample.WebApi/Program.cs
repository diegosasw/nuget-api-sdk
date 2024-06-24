using WebApiOne.Client.DependencyInjection;
using WebApiOne.Client.Sdk;
using WebApiOne.Client.Sdk.Models;

var builder = WebApplication.CreateBuilder(args);

// Register SDK in DI Container
builder.Services.AddWebApiOneClient("SdkWebApiOneSample");

var app = builder.Build();

// Use SDK anywhere within the app by relying on DI Container injection
app.MapGet("/", async (WebApiOneClient webApiOneClient) =>
{
    var project =
        new Project
        {
            Name = "Sample Project", 
            Members = [
                new Member
                {
                    Name = "Joe Bloggs",
                    Email = "joe@test.com",
                    IsDeveloper = false
                },
                new Member
                {
                    Name = "Jane Smith",
                    Email = "jane@test.com",
                    IsDeveloper = true
                }
            ]
        };

    await using var stream = await webApiOneClient.Projects.PostAsync(project);
    using var reader = new StreamReader(stream ?? throw new InvalidOperationException());
    var result = await reader.ReadToEndAsync();
    return Results.Text(result);
});

app.Run();