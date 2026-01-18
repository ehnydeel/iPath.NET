using iPath.Application.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace iPath.Application.Features.Notifications;


/*
 * This queue is used to transform DomainEvents of type NodeEvent to Notifications
 * Events are handled by NotificationEventHandler and then placed on this queue
 * for further processing => filter by user subscription, etc ...
 * 
 */

public class EventNotificationDispatcherQueue : IEventNotificationDispatcherQueue
{
    private readonly Channel<ServiceRequestEvent> _channel;

    public EventNotificationDispatcherQueue(int maxQueueSize)
    {
        _channel = Channel.CreateBounded<ServiceRequestEvent>(new BoundedChannelOptions(maxQueueSize)
        {
            SingleReader = false,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public async ValueTask EnqueueAsync(ServiceRequestEvent item)
    {
        if (item != null)
            await _channel.Writer.WriteAsync(item);
    }

    public async ValueTask<ServiceRequestEvent> DequeueAsync(CancellationToken cancellationToken)
    {
        var item = await _channel.Reader.ReadAsync(cancellationToken);
        return item;
    }

    public int QueueSize => _channel.Reader.Count;
}
