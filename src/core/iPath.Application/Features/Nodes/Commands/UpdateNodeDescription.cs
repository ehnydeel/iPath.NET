namespace iPath.Application.Features.Nodes;

public record UpdateNodeCommand(Guid NodeId, NodeDescription? Description, bool? IsDraft)
    : IRequest<UpdateNodeCommand, Task<bool>>
    , IEventInput
{
    public string ObjectName => nameof(Node);
}

