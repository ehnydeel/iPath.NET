namespace iPath.Application.Features.Notifications;

public class NotificationEventHandler(NotificationEventQueue queue)
    : INotificationHandler<EventEntity>
{
    public async ValueTask Handle(EventEntity evt, CancellationToken cancellationToken)
    {
        if (evt is IHasNodeNotification ne)
            await queue.EnqueueAsync(ne);
    }
}