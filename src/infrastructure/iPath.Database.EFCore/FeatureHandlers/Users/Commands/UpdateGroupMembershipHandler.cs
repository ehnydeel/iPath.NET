using iPath.Application.Exceptions;

namespace iPath.EF.Core.FeatureHandlers.Users.Commands;

public class UpdateGroupMembershipHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<UpdateGroupMembershipCommand, Task>
{
    public async Task Handle(UpdateGroupMembershipCommand request, CancellationToken ct)
    {
        var user = await db.Users.FindAsync(request.UserId, ct);
        Guard.Against.NotFound(request.UserId, user);

        var set = db.Set<GroupMember>();

        // Validate that User is allowed to modify groups
        if (sess.IsAdmin)
        {
            // admin may update all
        }
        else if (sess.IsModerator) 
        {
            // Moderator mayb update only the Grouops for which they have moderator role
            foreach (var item in request.Membership)
            {
                if (!sess.IsGroupModerator(item.GroupId)) throw new NotAllowedException();
            }
        }
        else
        {
            throw new NotAllowedException();
        }


        // reload from DB
        var groupIds = request.Membership.Select(m => m.GroupId).ToHashSet();
        var list = await set
            .Where(m => m.UserId == request.UserId)
            .Where(m => groupIds.Contains(m.GroupId))
            .ToListAsync(ct);

        // remove those set to None
        foreach (var entity in list)
        {
            if (request.Membership.Where(m => m.Role == eMemberRole.None).Any(dto => dto.GroupId == entity.GroupId))
            {
                set.Remove(entity);
            }
        }

        // update and add new
        foreach (var dto in request.Membership)
        {
            var entity = list.FirstOrDefault(m => m.GroupId == dto.GroupId);
            if (entity == null)
            {
                entity = new GroupMember()
                {
                    UserId = request.UserId,
                    GroupId = dto.GroupId,
                };
                await set.AddAsync(entity, ct);
            }
            entity.Role = dto.Role;
        }

        // User is derived from Identity User and thus does not have domain events => save events directly
        var evt = EventEntity.Create<GroupMembershipUpdatedEvent, UpdateGroupMembershipCommand>(request, user.Id, sess.User.Id);
        await db.EventStore.AddAsync(evt, ct);

        await db.SaveChangesAsync(ct);
    }
}