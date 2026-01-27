namespace iPath.Application.Contracts;

public interface IServiceRequestEventQueue
{
    int QueueSize { get; }

    ValueTask<ServiceRequestEvent> DequeueAsync(CancellationToken cancellationToken);
    ValueTask EnqueueAsync(ServiceRequestEvent item);
}