namespace iPath.Application.Contracts;

public interface INodeNotificationEventQueue
{
    int QueueSize { get; }

    ValueTask<INodeNotificationEvent> DequeueAsync(CancellationToken cancellationToken);
    ValueTask EnqueueAsync(INodeNotificationEvent item);
}