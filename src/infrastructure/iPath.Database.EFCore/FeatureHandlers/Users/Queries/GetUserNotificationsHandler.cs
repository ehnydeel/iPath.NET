namespace iPath.EF.Core.FeatureHandlers.Users.Queries;

public class GetUserNotificationsHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<GetUserNotificationsQuery, Task<IEnumerable<UserGroupNotificationDto>>>
{
    public async Task<IEnumerable<UserGroupNotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken ct)
    {
        // authorize
        if (!sess.IsAdmin && sess.User.Id != request.UserId)
            throw new NotAllowedException();

        var user = await db.Users
            .Include(u => u.GroupMembership).ThenInclude(m => m.Group)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == request.UserId, ct);
        Guard.Against.NotFound(request.UserId, user);

        var items = user.GroupMembership 
            //.Where(m => m.NotificationSource != eNotificationSource.None)
            .Select(m => new UserGroupNotificationDto(UserId: user.Id,
            GroupId: m.Group.Id, Groupname: m.Group.Name,
            Source: m.NotificationSource, Tartget: m.NotificationTarget, Settings: m.NotificationSettings));

        return items;
    }
}
