using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using iPath.Application;
using iPath.API;
using iPath.EF.Core.Database;

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
                services.AddIPathAPI(config);
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
