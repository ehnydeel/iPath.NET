namespace iPath.Application.Features.ServiceRequests;


public class ChildNodeCreatedEvent : ServiceRequestEvent, INotification
{
    public Guid? RootParentId { get; set; }
}


