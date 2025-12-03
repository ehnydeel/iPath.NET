using iPath.Domain.Notificxations;

namespace iPath.Application.Features.Nodes;

public class RootNodePublishedEvent : NodeEvent, INotification, INodeNotificationEvent
{
    public eNodeEventType EventType => eNodeEventType.NodePublished;

    public NodeEvent Event => this;
}
