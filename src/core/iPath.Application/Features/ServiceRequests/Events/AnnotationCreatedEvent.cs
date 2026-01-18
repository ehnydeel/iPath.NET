using iPath.Domain.Notificxations;

namespace iPath.Application.Features.ServiceRequests;

public class AnnotationCreatedEvent : ServiceRequestEvent, INotification
{
    public ServiceRequestEvent Event => this;
}
