namespace iPath.Application.Features.Admin;

public class GetEventsQuery : PagedQuery<EventEntity>
    , IRequest<GetEventsQuery, Task<PagedResultList<EventEntity>>>
{
    public string? EventType { get; init; }
    public string? ObjectName { get; init; }
    public Guid? ObjectId { get; init; }
}
