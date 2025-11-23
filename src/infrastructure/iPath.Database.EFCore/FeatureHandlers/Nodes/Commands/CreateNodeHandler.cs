public class CreateNodeCommandHandler(iPathDbContext db, IUserSession sess, IMediator mediator)
    : IRequestHandler<CreateNodeCommand, Task<NodeDto>>
{
    public async Task<NodeDto> Handle(CreateNodeCommand request, CancellationToken ct)
    {
        if (!sess.IsAdmin)
            sess.AssertInGroup(request.GroupId);

        var group = await db.Groups.FindAsync(request.GroupId, ct);
        Guard.Against.NotFound(request.GroupId, group);

        var node = NodeCommandExtensions.CreateNode(request, sess.User.Id);
        await db.Nodes.AddAsync(node, ct);
        await db.SaveChangesAsync(ct);

        return node.ToDto();
    }
}