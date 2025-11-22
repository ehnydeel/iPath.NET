namespace iPath.Application.Features.Nodes;

public record CreateNodeCommand(Guid GroupId, string NodeType, NodeDescription? Description = null, Guid? NodeId = null)
    : IRequest<CreateNodeCommand, Task<NodeDto>>
    , IEventInput
{
    public string ObjectName => nameof(Node);
}
