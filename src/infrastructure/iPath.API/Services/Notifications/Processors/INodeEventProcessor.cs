using iPath.Domain.Notificxations;

namespace iPath.API.Services.Notifications.Processors;

public interface INodeEventProcessor
{
    Task ProcessEvent(ServiceRequestEvent n, CancellationToken ct);
}
