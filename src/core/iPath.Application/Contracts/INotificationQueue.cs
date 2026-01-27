namespace iPath.Application.Contracts;

public interface INotificationQueue
{
    ValueTask EnqueueAsync(Guid id);

    ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken);

    int QueueSize { get; }
}
