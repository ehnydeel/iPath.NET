namespace iPath.EF.Core.FeatureHandlers.Nodes.Commands;

public class UpdateNodeVisitCommandHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<UpdateServiceRequestVisitCommand, Task<bool>>
{
    public async Task<bool> Handle(UpdateServiceRequestVisitCommand request, CancellationToken ct)
    {
        Guard.Against.Null(sess.User);

        var node = await db.ServiceRequests.FindAsync(request.NodeId);
        Guard.Against.NotFound(request.NodeId, node);

        var v = await db.Set<NodeLastVisit>().FirstOrDefaultAsync(v => v.UserId == sess.User.Id && v.NodeId == request.NodeId, ct);
        if (v != null)
        {
            v.Date = DateTime.UtcNow;
            db.Set<NodeLastVisit>().Update(v);
        }
        else
        {
           v = NodeLastVisit.Create(userId: sess.User.Id, nodeId: request.NodeId, date: DateTime.UtcNow);
           await db.Set<NodeLastVisit>().AddAsync(v, ct);
        }
        await db.SaveChangesAsync(ct);

        return true;
    }
}
