using iPath.Application.Features.Notifications;
using iPath.EF.Core.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace iPath.API.Services.Notifications;

public class NotificationQueueWorker(NotificationEventQueue queue,
    ILogger<NotificationQueueWorker> logger,
    IServiceProvider services)
    : BackgroundService
{
    private IServiceScope scope;
    private IMediator mediator;
    private iPathDbContext db;


    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        scope = services.CreateScope();
        db = scope.ServiceProvider.GetRequiredService<iPathDbContext>();
        mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        scope.Dispose();
        scope = null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NotificationQueueWorker Hosted Service is running.");
        await BackgroundProcessing(stoppingToken);
    }

    private async Task BackgroundProcessing(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var evt = await queue.DequeueAsync(stoppingToken);
            logger.LogTrace("processing event");

            await Task.Delay(500);
        }
    }
}
