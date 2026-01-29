using iPath.Application.Features.Admin;

namespace iPath.EF.Core.FeatureHandlers.Admin;


public class GetEventsQueryHandler(iPathDbContext db)
    : IRequestHandler<GetEventsQuery, Task<PagedResultList<EventEntity>>>
{
    public async Task<PagedResultList<EventEntity>> Handle(GetEventsQuery request, CancellationToken ct)
    {
        var q = db.EventStore.AsNoTracking()
            .ApplyQuery(request, "EventDate DESC");
        return await q.ToPagedResultAsync(request, ct);
    }
}
