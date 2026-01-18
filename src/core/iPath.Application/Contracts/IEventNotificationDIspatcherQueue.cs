namespace iPath.Application.Contracts;

public interface IEventNotificationDispatcherQueue
{
    int QueueSize { get; }

    ValueTask<ServiceRequestEvent> DequeueAsync(CancellationToken cancellationToken);
    ValueTask EnqueueAsync(ServiceRequestEvent item);
}