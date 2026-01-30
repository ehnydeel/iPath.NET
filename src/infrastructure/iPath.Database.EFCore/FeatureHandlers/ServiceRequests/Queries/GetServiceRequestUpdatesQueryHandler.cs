
namespace iPath.EF.Core.FeatureHandlers.ServiceRequests.Queries;


public class GetServiceRequestUpdatesQueryHandler (iPathDbContext db, IUserSession sess)
    : IRequestHandler<GetServiceRequestUpdatesQuery, Task<ServiceRequestUpdatesDto>>
{
    public async Task<ServiceRequestUpdatesDto> Handle(GetServiceRequestUpdatesQuery request, CancellationToken cancellationToken)
    {
        var minDate = DateTime.UtcNow.AddYears(-1);

        var q = db.ServiceRequests.AsNoTracking()
            .Where(n => sess.GroupIds().Contains(n.GroupId));
        if (request.CommunityId.HasValue)
        {
            q = q.Where(n => n.Group.CommunityId == request.CommunityId.Value);
        }

        var newrequests = await q
            .Where(n => !n.IsDraft && n.CreatedOn > minDate && !n.LastVisits.Any(v => v.UserId == sess.User.Id))
            .Select(n => new ServiceRequestIds(Id: n.Id, GroupId: n.GroupId, BodySiteCode: n.Description.BodySite.Code))
            .ToListAsync(cancellationToken);

        var newannotations = await q
            .Where(n => !n.IsDraft && n.Annotations.Any(a => a.CreatedOn > minDate && a.OwnerId != sess.User.Id &&
                   (!n.LastVisits.Any(v => v.UserId == sess.User.Id) || a.CreatedOn > n.LastVisits.First(v => v.UserId == sess.User.Id).Date)))
            .Select(n => new ServiceRequestIds(Id: n.Id, GroupId: n.GroupId, BodySiteCode: n.Description.BodySite.Code))
            .ToListAsync(cancellationToken);

        return new ServiceRequestUpdatesDto
        {
            NewRequests = newrequests,
            NewAnnotations = newannotations
        };
    }
}
