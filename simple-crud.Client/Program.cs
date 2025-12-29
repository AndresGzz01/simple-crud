using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using simple_crud.Client;
using simple_crud.Client.Infrastructure;
using simple_crud.Client.Models;
using simple_crud.Client.Validator.User;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var client = new HttpClient
    {
        BaseAddress = new Uri("https://simple-crud.api.andresgilglz.dev/api/")
    };

    client.DefaultRequestHeaders.Add("Accept", "application/json");
    return client;
});

builder.Services.AddScoped<IBlogService, LabsystecBlogService>();

builder.Services.AddScoped<CreateUserValidator>();
builder.Services.AddScoped<LoginValidator>();

builder.Services.AddSingleton<LoaderService>();

await builder.Build().RunAsync();
