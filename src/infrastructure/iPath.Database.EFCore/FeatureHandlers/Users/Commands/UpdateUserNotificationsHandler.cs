using iPath.Application.Exceptions;

namespace iPath.Application.Features;

public class UpdateUserNotificationsHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<UpdateUserNotificationsCommand, Task>
{
    public async Task Handle(UpdateUserNotificationsCommand request, CancellationToken ct)
    {
        var user = await db.Users.FindAsync(request.UserId, ct);
        Guard.Against.NotFound(request.UserId, user);

        // validate that session user is admin or equals user to modify
        if (!sess.CanModifyUser(request.UserId))
        { 
            throw new NotAllowedException("You are not allowed to update notifications of another user");
        }


        var set = db.Set<GroupMember>();

        // reload from DB
        var list = await set.Where(m => m.UserId == request.UserId).ToListAsync(ct);

        // remove those set to None
        foreach (var entity in list)
        {
            var dto = request.Notifications.FirstOrDefault(n => n.GroupId == entity.GroupId);
            eNotificationSource n = dto is null ? eNotificationSource.None : dto.Source;
            if( entity.Notifications != n)
            {
                entity.Notifications = n;
                entity.NotificationTargets = dto.Tartget;
                entity.NotificationFilter = dto.Filter;
                db.Update(entity);
            }
        }

        // User is derived from Identity User and thus does not have domain events => save events directly
        var evt = EventEntity.Create<UserNotificationsUpdatedEvent, UpdateUserNotificationsCommand>(request, user.Id, sess.User.Id);
        await db.EventStore.AddAsync(evt, ct);

        await db.SaveChangesAsync(ct);
    }
}