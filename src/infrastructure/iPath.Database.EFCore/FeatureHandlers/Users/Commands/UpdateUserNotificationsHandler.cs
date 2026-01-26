namespace iPath.Application.Features;

public class UpdateUserNotificationsHandler(iPathDbContext db, IMediator mediator, IUserSession sess)
    : IRequestHandler<UpdateUserNotificationsCommand, Task<UserDto>>
{
    public async Task<UserDto> Handle(UpdateUserNotificationsCommand request, CancellationToken ct)
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
            if (dto is not null)
            {
                entity.NotificationSource = dto.Source;
                entity.NotificationTarget = dto.Tartget;
                entity.NotificationSettings = dto.Settings;
            }
        }

        // User is derived from Identity User and thus does not have domain events => save events directly
        var evt = EventEntity.Create<UserNotificationsUpdatedEvent, UpdateUserNotificationsCommand>(request, user.Id, sess.User.Id);
        await db.EventStore.AddAsync(evt, ct);

        await db.SaveChangesAsync(ct);

        return await mediator.Send(new GetUserByIdQuery(user.Id), ct);
    }
}