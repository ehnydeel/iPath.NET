namespace iPath.Application.Features.ServiceRequests;



public class GetServiceRequestIdListQuery : PagedQuery<ServiceRequestListDto>
    , IRequest<GetServiceRequestIdListQuery, Task<IReadOnlyList<Guid>>>
{
    public GetServiceRequestIdListQuery()
    {   
    }

    public GetServiceRequestIdListQuery(GetServiceRequestsQuery q) 
    { 
        GroupId = q.GroupId;
        OwnerId = q.OwnerId;
        Sorting = q.Sorting;
        Filter = q.Filter;
        PageSize = null;
        Page = 0;
    }

    public Guid? GroupId { get; set; }
    public Guid? OwnerId { get; set; }
    public bool inclDrafts { get; set; } = false;
}
