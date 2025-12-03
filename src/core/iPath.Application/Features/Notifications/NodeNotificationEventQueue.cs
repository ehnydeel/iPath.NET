using iPath.Application.Contracts;
using System.Threading.Channels;

namespace iPath.Application.Features.Notifications;

public class NodeNotificationEventQueue : INodeNotificationEventQueue
{
    private readonly Channel<INodeNotificationEvent> _channel;

    public NodeNotificationEventQueue(int maxQueueSize)
    {
        _channel = Channel.CreateBounded<INodeNotificationEvent>(new BoundedChannelOptions(maxQueueSize)
        {
            SingleReader = false,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public async ValueTask EnqueueAsync(INodeNotificationEvent item)
    {
        if (item != null)
            await _channel.Writer.WriteAsync(item);
    }

    public async ValueTask<INodeNotificationEvent> DequeueAsync(CancellationToken cancellationToken)
    {
        var item = await _channel.Reader.ReadAsync(cancellationToken);
        return item;
    }

    public int QueueSize => _channel.Reader.Count;
}
