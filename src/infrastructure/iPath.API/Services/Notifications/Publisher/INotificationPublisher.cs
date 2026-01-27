namespace iPath.API.Services.Notifications.Publisher;

public interface INotificationPublisher
{
    eNotificationTarget Target { get; }

    Task PublishAsync(Notification n, CancellationToken ct);
}
