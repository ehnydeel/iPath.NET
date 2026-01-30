namespace iPath.Application.Features.ServiceRequests;

public class GetServiceRequestUpdatesQuery : IRequest<GetServiceRequestUpdatesQuery, Task<ServiceRequestUpdatesDto>>
{
    public Guid? CommunityId { get; set; }
}

public class GetNewServiceRequestsQuery : PagedQuery<ServiceRequestListDto>
    , IRequest<GetNewServiceRequestsQuery, Task<PagedResultList<ServiceRequestListDto>>>
{
    public Guid? CommunityId { get; set; }
}


public class GetServiceRequestsWithNewAnnotationsQuery : PagedQuery<ServiceRequestListDto>
    , IRequest<GetServiceRequestsWithNewAnnotationsQuery, Task<PagedResultList<ServiceRequestListDto>>>
{
    public Guid? CommunityId { get; set; }
}
