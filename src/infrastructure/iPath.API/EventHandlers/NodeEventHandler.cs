using DispatchR.Abstractions.Notification;
using iPath.API.Hubs;
using iPath.Domain.Notificxations;
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
        if (evt is INodeNotificationEvent ne)
        {
            var msg = new NotificationMessage(evt.EventDate, ne.EventType);
            await hub.Clients.All.SendAsync("NodeEvent", msg);
        }
        else 
        { 
            logger.LogWarning("Unhandled domain event {0}", evt.GetType().Name);
        }
    }
}

public record NotificationMessage(DateTime Date, eNodeEventType EventType);