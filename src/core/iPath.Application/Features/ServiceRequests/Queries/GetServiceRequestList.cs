namespace iPath.Application.Features.ServiceRequests;



public class GetServiceRequestsQuery : PagedQuery<ServiceRequestListDto>
    , IRequest<GetServiceRequestsQuery, Task<PagedResultList<ServiceRequestListDto>>>
{
    public Guid? GroupId { get; set; }
    public Guid? OwnerId { get; set; }

    public bool IncludeDetails { get; set; }
    public string SearchString { get; set; }
}
