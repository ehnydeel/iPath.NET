using iPath.Application.Features.CMS;

namespace iPath.EF.Core.FeatureHandlers.CMS.Commands;

public class DeleteWebContentCommandHandler(iPathDbContext db, IUserSession sess)
    : IRequestHandler<DeleteWebContentCommand, Task>
{
    public async Task Handle(DeleteWebContentCommand request, CancellationToken cancellationToken)
    {
        if (!sess.IsAdmin)
        {
            throw new NotAllowedException();
        }
        var content = await db.WebPages.FirstOrDefaultAsync(x => x.Id == request.Id);
        Guard.Against.NotFound(request.Id, content);

        db.WebPages.Remove(content);

        await db.SaveChangesAsync(cancellationToken);
    }
}