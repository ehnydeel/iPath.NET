using iPath.Application.Fhir;
using iPath.Blazor.Server;
using iPath.Domain.Config;
using iPath.RazorLib;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// get client config from api
try
{
    var http = new HttpClient()
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        Timeout = TimeSpan.FromSeconds(5)
    };

    using var response = await http.GetAsync("api/v1/config");
    using var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);

    builder.Services.Configure<iPathClientConfig>(builder.Configuration.GetSection(iPathClientConfig.ConfigName));
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    // throw ex;
}


builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddTransient<baseAuthDelegationHandler>();

var baseAddress = builder.Configuration["BaseAddress"] ?? builder.HostEnvironment.BaseAddress;
Console.WriteLine("Blazor WASM starting with Base: " + baseAddress);
await builder.Services.AddRazorLibServices(baseAddress, true);

await builder.Build().RunAsync();
