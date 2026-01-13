using iPath.Application.Features.Notifications;
using iPath.EF.Core.Database;
using iPath.EF.Core.FeatureHandlers.Emails;
using iPath.EF.Core.FeatureHandlers.Groups;
using iPath.EF.Core.FeatureHandlers.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using iPath.Google;


namespace iPath.API;

public static class PersistanceServiceRegistration
{
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<iPathDbContext>(cfg =>
        {
            var provider = config.GetSection("DbProvider").Value ?? DBProvider.Sqlite.Name; // read from appsettings and default to sqlite
            Console.WriteLine("DbProvider = " + provider);

            if (provider == DBProvider.Postgres.Name)
            {
                cfg.UseNpgsql(
                    config.GetConnectionString(DBProvider.Postgres.Name),
                    x => x.MigrationsAssembly(DBProvider.Postgres.Assembly)
                );
            }
            else if (provider == DBProvider.SqlServer.Name)
            {
                var cs = config.GetConnectionString(DBProvider.SqlServer.Name);
                cfg.UseSqlServer(
                    config.GetConnectionString(DBProvider.SqlServer.Name),
                    x => x.MigrationsAssembly(DBProvider.SqlServer.Assembly)
                );
            }
            else if (provider == DBProvider.Sqlite.Name)
            {
                var cs = config.GetConnectionString(DBProvider.Sqlite.Name);
                cfg.UseSqlite(
                    config.GetConnectionString(DBProvider.Sqlite.Name) ?? "ipath.db", // default to ipath.db
                    x => x.MigrationsAssembly(DBProvider.Sqlite.Assembly)
                );
            }
            else
            {
                throw new Exception("no db provider configuration found");
            }
        });

        // services.AddDbFactory(config);

        services.AddScoped<DbSeeder>();
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IGroupService, GroupService>();

        // Google Workspace
        services.AddGoogleServices(config);


        return services;
    }

    private static IServiceCollection AddDbFactory(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContextFactory<iPathDbContext>(cfg =>
        {
            var provider = config.GetSection("DbProvider").Value ?? DBProvider.Postgres.Name;
            // Console.WriteLine(provider);

            if (provider == DBProvider.Postgres.Name)
            {
                cfg.UseNpgsql(
                    config.GetConnectionString(DBProvider.Postgres.Name),
                    x => x.MigrationsAssembly(DBProvider.Postgres.Assembly)
                );
            }
            if (provider == DBProvider.SqlServer.Name)
            {
                var cs = config.GetConnectionString(DBProvider.SqlServer.Name);
                cfg.UseSqlServer(
                    config.GetConnectionString(DBProvider.SqlServer.Name),
                    x => x.MigrationsAssembly(DBProvider.SqlServer.Assembly)
                );
            }
            if (provider == DBProvider.Sqlite.Name)
            {
                cfg.UseSqlite(
                    config.GetConnectionString(DBProvider.Sqlite.Name),
                    x => x.MigrationsAssembly(DBProvider.Sqlite.Assembly)
                );
            }
        });

        return services;
    }

    public static async Task UpdateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

        await seeder.UpdateDatabase();
    }
}


public record DBProvider(string Name, string Assembly)
{
    public static DBProvider Sqlite = new(nameof(Sqlite), typeof(iPath.EF.Sqlite.Marker).Assembly.GetName().Name!);
    public static DBProvider Postgres = new(nameof(Postgres), typeof(iPath.EF.Postgres.Marker).Assembly.GetName().Name!);
    public static DBProvider SqlServer = new(nameof(SqlServer), typeof(iPath.EF.SqlServer.Marker).Assembly.GetName().Name!);
}