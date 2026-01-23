using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using iPath.Application;
using iPath.API;
using iPath.EF.Core.Database;
using Microsoft.Extensions.DependencyInjection;
using iPath.Application.Fhir;
using iPath.Blazor.ServiceLib.Fhir;

namespace iPath.Test.xUnit2;

public class iPathFixture : IDisposable
{
    public readonly IServiceProvider ServiceProvider;

    public iPathFixture()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                IConfiguration config = context.Configuration;
                services.AddSingleton<IConfiguration>(config);  
                services.AddIPathAPI(config);
                services.AddSingleton<IFhirDataLoader, FileFhirDataLoader>();
            });

        var host = builder.Build();
        ServiceProvider = host.Services;      
    }

    public void Dispose()
    {
        var ctx = ServiceProvider.GetService(typeof(iPathDbContext)) as iPathDbContext;
        ctx.Database.EnsureDeleted();
    }
}
