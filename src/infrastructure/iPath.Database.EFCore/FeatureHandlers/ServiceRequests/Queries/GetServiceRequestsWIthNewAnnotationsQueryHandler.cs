
namespace iPath.EF.Core.FeatureHandlers.ServiceRequests.Queries;

public class GetServiceRequestsWithNewAnnotationsQueryHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<GetServiceRequestsWithNewAnnotationsQuery, Task<PagedResultList<ServiceRequestListDto>>>
{
    public async Task<PagedResultList<ServiceRequestListDto>> Handle(GetServiceRequestsWithNewAnnotationsQuery request, CancellationToken cancellationToken)
    {
        var minDate = DateTime.UtcNow.AddYears(-1);

        var q = db.ServiceRequests.AsNoTracking()
            .Where(n => sess.GroupIds().Contains(n.GroupId));
        if (request.CommunityId.HasValue)
        {
            q = q.Where(n => n.Group.CommunityId == request.CommunityId.Value);
        }

        q = q.Where(n => !n.IsDraft && n.Annotations.Any(a => a.CreatedOn > minDate && a.OwnerId != sess.User.Id &&
                 (!n.LastVisits.Any(v => v.UserId == sess.User.Id) || a.CreatedOn > n.LastVisits.First(v => v.UserId == sess.User.Id).Date)));

        var projected = q.ProjectToListDetails(sess.User.Id);

        // paginate
        return await projected.ToPagedResultAsync(request, cancellationToken);
    }
}