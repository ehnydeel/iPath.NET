using DispatchR.Abstractions.Notification;
using iPath.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace iPath.API;


public class DomainEventHandler(IHubContext<NodeNotificationsHub> hub,
    IUserSession sess,
    ILogger<DomainEventHandler> logger)
    : INotificationHandler<EventEntity>
{
    public async ValueTask Handle(EventEntity evt, CancellationToken ct)
    {
        if (evt is IHasNodeNotification ne)
        {
            await hub.Clients.All.SendAsync("NodeEvent", ne.ToNotification());
        }
        else 
        { 
            logger.LogWarning("Unhandled domain event {0}", evt.GetType().Name);
        }
    }

}