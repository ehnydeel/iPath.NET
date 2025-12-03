using iPath.Domain.Notificxations;

namespace iPath.API.Services.Notifications.Processors;

public interface INodeEventProcessor
{
    Task ProcessEvent(INodeNotificationEvent n, CancellationToken ct);
}
