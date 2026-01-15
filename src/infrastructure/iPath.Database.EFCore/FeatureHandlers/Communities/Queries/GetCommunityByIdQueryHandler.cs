namespace iPath.EF.Core.FeatureHandlers.Communities.Queries;

public class GetCommunityByIdQueryHandler(iPathDbContext db)
    : IRequestHandler<GetCommunityByIdQuery, Task<CommunityDto>>
{
    public async Task<CommunityDto> Handle(GetCommunityByIdQuery request, CancellationToken cancellationToken)
    {
        var community = await db.Communities.AsNoTracking()
            .Where(c => c.Id == request.id)
            .Select(c => new CommunityDto(Id: c.Id, Name: c.Name, Settings: c.Settings, Visibility: c.Visibility,
                Owner: c.Owner.ToOwnerDto(),
                Groups: c.Groups.Select(g => new GroupListDto(g.Id, g.Name, Visibility: g.Visibility)).ToArray(),
                ExtraGroups: c.ExtraGroups.Select(g => new GroupListDto(g.Group.Id, g.Group.Name, Visibility: g.Group.Visibility)).ToArray()))
            .FirstOrDefaultAsync(cancellationToken);
        Guard.Against.NotFound(request.id, community);
        return community;
    }
}

