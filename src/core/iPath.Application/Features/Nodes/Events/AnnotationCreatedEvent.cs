using iPath.Domain.Notificxations;

namespace iPath.Application.Features.Nodes;

public class AnnotationCreatedEvent : NodeEvent, INotification, INodeNotificationEvent
{
    public eNodeEventType EventType => eNodeEventType.NewAnnotation;

    public NodeEvent Event => this;
}
