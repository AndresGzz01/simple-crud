using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using simple_crud.Client;
using simple_crud.Client.Infrastructure;
using simple_crud.Client.Models;

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

await builder.Build().RunAsync();
