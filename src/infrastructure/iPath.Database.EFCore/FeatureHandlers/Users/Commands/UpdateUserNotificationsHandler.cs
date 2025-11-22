using iPath.Application.Exceptions;

namespace iPath.Application.Features;

public class UpdateUserNotificationsHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<UpdateUserNotificationsInput, Task<bool>>
{
    public async Task<bool> Handle(UpdateUserNotificationsInput request, CancellationToken ct)
    {
        var user = await db.Users.FindAsync(request.UserId, ct);
        Guard.Against.NotFound(request.UserId, user);

        // validate that session user is admin or equals user to modify
        if (!sess.CanModifyUser(request.UserId))
        { 
            throw new NotAllowedException("You are not allowed to update notifications of another user");
        }

        await using var trans = await db.Database.BeginTransactionAsync(ct);

        var set = db.Set<GroupMember>();

        // reload from DB
        var list = await set.Where(m => m.UserId == request.UserId).ToListAsync(ct);

        // remove those set to None
        foreach (var entity in list)
        {
            var dto = request.Notifications.FirstOrDefault(n => n.GroupId == entity.GroupId);
            eNotification n = dto is null ? eNotification.None : dto.Notifications;
            if( entity.Notifications != n)
            {
                entity.Notifications = n;
                db.Update(entity);
            }
        }

        var evt = EventEntity.Create<UserNotificationsUpdatedEvent, UpdateUserNotificationsInput>(request, user.Id, sess.User.Id);
        await db.EventStore.AddAsync(evt, ct);

        await db.SaveChangesAsync(ct);
        await trans.CommitAsync();

        return true;
    }
}