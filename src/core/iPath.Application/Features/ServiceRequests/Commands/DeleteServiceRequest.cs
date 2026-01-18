namespace iPath.Application.Features.ServiceRequests;

public record DeleteServiceRequestCommand(Guid NodeId)
    : IRequest<DeleteServiceRequestCommand, Task<ServiceRequestDeletedEvent>>
    , IEventInput
{
    public string ObjectName => nameof(ServiceRequest);
}

