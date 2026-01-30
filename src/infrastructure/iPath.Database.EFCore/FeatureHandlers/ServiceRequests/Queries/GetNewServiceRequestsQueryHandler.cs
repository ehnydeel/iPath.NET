
namespace iPath.EF.Core.FeatureHandlers.ServiceRequests.Queries;

public class GetNewServiceRequestsQueryHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<GetNewServiceRequestsQuery, Task<PagedResultList<ServiceRequestListDto>>>
{
    public async Task<PagedResultList<ServiceRequestListDto>> Handle(GetNewServiceRequestsQuery request, CancellationToken cancellationToken)
    {
        var minDate = DateTime.UtcNow.AddYears(-1);

        var q = db.ServiceRequests.AsNoTracking()
            .Where(n => sess.GroupIds().Contains(n.GroupId));
        if (request.CommunityId.HasValue)
        {
            q = q.Where(n => n.Group.CommunityId == request.CommunityId.Value);
        }

        q = q.Where(n => !n.IsDraft && n.CreatedOn > minDate && !n.LastVisits.Any(v => v.UserId == sess.User.Id));

        var projected = q.ProjectToListDetails(sess.User.Id);

        // paginate
        return await projected.ToPagedResultAsync(request, cancellationToken);
    }
}