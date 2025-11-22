namespace iPath.EF.Core.FeatureHandlers.Nodes.Commands;


public class CreateNodeAnnotationCommandHandler(iPathDbContext db, IMediator mediator, IUserSession sess)
    : IRequestHandler<CreateNodeAnnotationCommand, Task<AnnotationDto>>
{
    public async Task<AnnotationDto> Handle(CreateNodeAnnotationCommand request, CancellationToken ct)
    {
        Guard.Against.NullOrEmpty(request.Text + request.QuestionnaireResponse);

        var node = await db.Nodes.FindAsync(request.NodeId);
        Guard.Against.NotFound(request.NodeId, node);

        if (!sess.IsAdmin)
        {
            // TODO: check authorization. Who may add Annotations ???

        }

        await using var tran = await db.Database.BeginTransactionAsync(ct);
        try
        {
            var a = new Annotation
            {
                Text = request.Text,
                OwnerId = sess.User.Id,
                CreatedOn = DateTime.UtcNow,
            };
            node.Annotations.Add(a);

            db.Nodes.Update(node);
            var evt = await db.CreateEventAsync<AnnotationCreatedEvent, CreateNodeAnnotationCommand, Node>(request, node);
            evt.GroupId = node.GroupId;
            await db.SaveChangesAsync(ct);

            await tran.CommitAsync(ct);

            return a.ToDto();
        }
        catch (Exception ex) 
        {
            await tran.RollbackAsync(ct);
        }
        return null;
    }
}