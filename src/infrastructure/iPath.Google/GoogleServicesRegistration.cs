using iPath.Application.Contracts;
using iPath.Google.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iPath.Google;

public static class GoogleServicesRegistration
{
    public static IServiceCollection AddGoogleServices(this IServiceCollection services, IConfiguration config)
    {
        var cfg = new GmailConfig();
        config.GetSection(nameof(GmailConfig)).Bind(cfg);

        if (cfg.Active)
        {
            services.Configure<GmailConfig>(config.GetSection(nameof(GmailConfig)));
            services.AddScoped<IMailBox, GmailIMapReader>();
        }

        return services;
    }
}