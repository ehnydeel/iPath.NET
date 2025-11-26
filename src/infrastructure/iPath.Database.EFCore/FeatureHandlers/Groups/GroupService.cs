using Microsoft.Extensions.Logging;

namespace iPath.EF.Core.FeatureHandlers.Groups;

public class GroupService(iPathDbContext db, IUserSession sess, ILogger<GroupService> logger)
    : IGroupService
{
    #region "-- Queries --"
    public async Task<GroupDto> GetGroupByIdAsync(Guid GroupId, CancellationToken ct = default)
    {
        sess.AssertInGroup(GroupId);

        var community = await db.Groups.AsNoTracking()
            .Where(g => g.Id == GroupId)
            .Select(g => new GroupDto(Id: g.Id, Name: g.Name, Settings: g.Settings,
                                      Members: g.Members.Select(m => new GroupMemberDto(UserId: m.User.Id, Username: m.User.UserName, Role: m.Role)).ToArray()))
            .FirstOrDefaultAsync(ct);

        Guard.Against.NotFound(GroupId, community);

        return community;
    }

    public async Task<PagedResultList<GroupListDto>> GetGroupListAsync(GetGroupListQuery request, CancellationToken ct = default)
    {
        var q = db.Groups.AsNoTracking();

        if (request.AdminList)
        {
            // only admins can get the full list
            sess.AssertInRole("Admin");
        }
        else if (sess.User is not null)
        {
            // users group list
            q = q.Where(g => sess.GroupIds().Contains(g.Id));
        }
        else
        {
            throw new NotAllowedException();
        }

        // filter
        q = q.ApplyQuery(request);

        // project
        IQueryable<GroupListDto> dtoQuery;
        if (!request.IncludeCounts)
        {
            dtoQuery = q.Select(x => new GroupListDto(x.Id, x.Name));
        }
        else
        {
            var minDate = DateTime.UtcNow.AddYears(-1);
            var uid = sess.User.Id;

            dtoQuery = q.Select(x => new GroupListDto(x.Id, x.Name,
                x.Nodes.Count(),
                x.Nodes.Count(n => n.CreatedOn > minDate && !n.LastVisits.Any(v => v.UserId == uid)),
                x.Nodes.Count(n => n.Annotations.Any(a => a.CreatedOn > minDate &&
                                                        (!n.LastVisits.Any(v => v.UserId == uid) || a.CreatedOn > n.LastVisits.First(v => v.UserId == uid).Date)))
                ));
        }

        // paginate
        var data = await dtoQuery.ToPagedResultAsync(request, ct);
        return data;
    }
    #endregion


    #region "-- Commands --"

    public async Task<GroupAssignedToCommunityEvent> AssignGroupToCommunityAsync(AssignGroupToCommunityCommand request, CancellationToken ct = default)
    {
        Guard.Against.Null(sess.User);
        sess.AssertInRole("Admin");

        var group = await db.Groups
            .Include(x => x.Communities)
            .FirstOrDefaultAsync(x => x.Id == request.GroupId, ct);
        Guard.Against.NotFound(request.GroupId.ToString(), group);

        var community = await db.Communities.FindAsync(new object[] { request.CommunityId }, ct);
        Guard.Against.NotFound(request.CommunityId.ToString(), community);


        // prepare changes
        if (request.Remove)
        {
            var toRemove = group.Communities.FirstOrDefault(x => x.CommunityId == request.CommunityId);
            if (toRemove != null)
            {
                group.Communities.Remove(toRemove);
            }
        }
        else
        {
            var exists = group.Communities.Any(x => x.CommunityId == request.CommunityId);
            if (!exists)
            {
                group.Communities.Add(new CommunityGroup()
                {
                    Group = group,
                    Community = community
                });
            }
        }

        await using var trans = await db.Database.BeginTransactionAsync(ct);
        try
        {
            var evt = EventEntity.Create<GroupAssignedToCommunityEvent, AssignGroupToCommunityCommand>(request, objectId: group.Id, userId: sess.User.Id);
            await db.EventStore.AddAsync(evt, ct);

            await db.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
            return evt;
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync();
        }
        return null;
    }

    public Task AssignQuestionnaireToGroupAsync(AssignQuestionnaireToGroupCommand cmd, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<GroupDto> CreateGroupAsync(CreateGroupCommand cmd, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteGroupAsync(DeleteGroupCommand cmd, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }



    public Task UpdateGroupAsync(UpdateGroupCommand cmd, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    #endregion
}
