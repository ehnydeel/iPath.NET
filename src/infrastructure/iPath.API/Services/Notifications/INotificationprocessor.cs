namespace iPath.API.Services.Notifications;

public interface INotificationProcessor
{
    eNotificationTarget Target { get; }
    Task<Result> ProcessNotificationAsync(Notification notification, CancellationToken cancellationToken = default);
}
