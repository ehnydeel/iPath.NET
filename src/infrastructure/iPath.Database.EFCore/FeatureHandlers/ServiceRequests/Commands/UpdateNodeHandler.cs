
using DispatchR;

namespace iPath.EF.Core.FeatureHandlers.Nodes.Commands;


public class UpdateServiceRequestHandler(iPathDbContext db, IMediator mediator, IUserSession sess)
    : IRequestHandler<UpdateServiceRequestCommand, Task<bool>>
{
    public async Task<bool> Handle(UpdateServiceRequestCommand request, CancellationToken ct)
    {
        var node = await db.ServiceRequests
            .SingleOrDefaultAsync(n => n.Id == request.NodeId, ct);
        Guard.Against.NotFound(request.NodeId, node);

        // permission
        if (!sess.IsAdmin)
        {
            if (!sess.IsGroupModerator(node.GroupId))
            {
                if (node.OwnerId != sess.User.Id)
                {
                    throw new NotAllowedException();
                }
            }
        }

        node.UpdateNode(request, sess.User.Id);
        await db.SaveChangesAsync(ct);

        // update user visit
        await mediator.Send(new UpdateServiceRequestVisitCommand(node.Id), ct);

        return true;
    }
}