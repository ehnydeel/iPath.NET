namespace iPath.EF.Core.FeatureHandlers.ServiceRequests;

using iPath.EF.Core.FeatureHandlers.ServiceRequests.Queries;
using EF = Microsoft.EntityFrameworkCore.EF;

public class GetNodesQueryHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<GetServiceRequestsQuery, Task<PagedResultList<ServiceRequestListDto>>>
{
    public async Task<PagedResultList<ServiceRequestListDto>> Handle(GetServiceRequestsQuery request, CancellationToken cancellationToken)
    {
        // prepare query (only root nodes)
        var q = db.ServiceRequests.AsNoTracking();

        if (request.GroupId.HasValue)
        {
            sess.AssertInGroup(request.GroupId.Value);
            q = q.Where(n => n.GroupId == request.GroupId.Value);
        }

        if (request.OwnerId.HasValue)
        {
            q = q.Where(n => n.OwnerId == request.OwnerId.Value);
        }

        // freetext search
        if (!string.IsNullOrEmpty(request.SearchString))
        {
            q = q.ApplySearchString(request.SearchString); 
        }

        if (request.Filter is not null)
        {
            foreach (var f in request.Filter)
            {
                q = q.ApplyNodeFilter(f);
            }
        }

        // Filter out drafts & private cases
        var spec = new NodeIsVisibleSpecifications(sess.IsAuthenticated ? sess.User.Id : null);
        q = q.Where(spec.ToExpression());


        // filter & sort
        q = q.ApplyQuery(request);

        // project
        IQueryable<ServiceRequestListDto> projected;
        if (!request.IncludeDetails)
        {
            projected = q.ProjectToList();
        }
        else
        {
            projected = q.ProjectToListDetails(sess.User.Id);
        }

        // paginate
        return await projected.ToPagedResultAsync(request, cancellationToken);
    }
}