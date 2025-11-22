namespace iPath.EF.Core.FeatureHandlers.Nodes.Commands;


public class UpdateNodeHandler(iPathDbContext db, IUserSession sess) 
    : IRequestHandler<UpdateNodeCommand, Task<bool>>
{
    public async Task<bool> Handle(UpdateNodeCommand request, CancellationToken ct)
    {      
        var node = await db.Nodes.FindAsync(request.NodeId, ct);
        Guard.Against.NotFound(request.NodeId, node);

        await using var tran = await db.Database.BeginTransactionAsync(ct);
        try
        {
            bool isPubish = false;
            if (node.IsDraft && request.IsDraft.HasValue && !request.IsDraft.Value)
                isPubish = true;

            if (request.Description is not null)
                node.Description = request.Description;
            if (request.IsDraft.HasValue)
                node.IsDraft = request.IsDraft.Value;

            node.LastModifiedOn = DateTime.UtcNow;
            await db.CreateEventAsync<NodeDescriptionUpdatedEvent, UpdateNodeCommand, Node>(request, node);
            if (isPubish)
            {
                var evt = await db.CreateEventAsync<RootNodePublishedEvent, UpdateNodeCommand, Node>(request, node);
                evt.GroupId = node.GroupId;
            }
            await db.SaveChangesAsync(ct);
            await tran.CommitAsync(ct);

            return true;
        }
        catch (Exception ex)
        {
            await tran.RollbackAsync(ct);
        }
        return false;
    }
}