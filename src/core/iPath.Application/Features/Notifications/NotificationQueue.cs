using iPath.Application.Contracts;
using System.Threading.Channels;

namespace iPath.Application.Features.Notifications;


/*
 * This queue is user to buffer the notifications for delivery to the user
 * ... place then on email queue or send over signalR to the client
 * 
 */


public class NotificationQueue : INotificationQueue
{
    private readonly Channel<Guid> _channel;

    public NotificationQueue(int maxQueueSize)
    {
        _channel = Channel.CreateBounded<Guid>(new BoundedChannelOptions(maxQueueSize)
        {
            SingleReader = false,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public async ValueTask EnqueueAsync(Guid item)
    {
        if (item != Guid.Empty)
            await _channel.Writer.WriteAsync(item);
    }

    public async ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken)
        => await _channel.Reader.ReadAsync(cancellationToken);

    public int QueueSize => _channel.Reader.Count;
}
