namespace iPath.Application.Features.ServiceRequests;

public record UpdateDcoumentsSortOrderCommand(Guid NodeId, Dictionary<Guid, int> sortOrder)
    : IRequest<UpdateDcoumentsSortOrderCommand, Task<ChildNodeSortOrderUpdatedEvent>>
    , IEventInput
{
    public string ObjectName => nameof(ServiceRequest);
}

