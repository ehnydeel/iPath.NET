namespace iPath.EF.Core.FeatureHandlers.ServiceRequests.Commands;

public class UpdateNodeVisitCommandHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<UpdateServiceRequestVisitCommand, Task<bool>>
{
    public async Task<bool> Handle(UpdateServiceRequestVisitCommand request, CancellationToken ct)
    {
        Guard.Against.Null(sess.User);

        var node = await db.ServiceRequests.FindAsync(request.NodeId);
        Guard.Against.NotFound(request.NodeId, node);

        var v = await db.Set<ServiceRequestLastVisit>().FirstOrDefaultAsync(v => v.UserId == sess.User.Id && v.ServiceRequestId == request.NodeId, ct);
        if (v != null)
        {
            v.Date = DateTime.UtcNow;
            db.Set<ServiceRequestLastVisit>().Update(v);
        }
        else
        {
           v = ServiceRequestLastVisit.Create(userId: sess.User.Id, nodeId: request.NodeId, date: DateTime.UtcNow);
           await db.Set<ServiceRequestLastVisit>().AddAsync(v, ct);
        }
        await db.SaveChangesAsync(ct);

        return true;
    }
}
