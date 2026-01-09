
using Microsoft.Extensions.Logging;

namespace iPath.EF.Core.FeatureHandlers.Nodes.Queries;


public class GetRootNodeByIdQueryHandler(iPathDbContext db, IUserSession sess, ILogger<GetRootNodeByIdQueryHandler> logger)
    : IRequestHandler<GetRootNodeByIdQuery, Task<NodeDto>>
{
    public async Task<NodeDto> Handle(GetRootNodeByIdQuery request, CancellationToken cancellationToken)
    {
        // Direct projection does not work with Sqlite => better call Entities in one query and project in memory
        var node = await db.Nodes.AsNoTracking()
            .Include(n => n.Owner)
            .Include(n => n.ChildNodes).ThenInclude(a => a.Owner)
            .Include(n => n.Annotations).ThenInclude(a => a.Owner)
            .AsSplitQuery()
            .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, node);
        Guard.Against.Null(node.GroupId);
        sess.AssertInGroup(node.GroupId.Value);

        var dto = node.ToDto();
        return dto;
    }
}
