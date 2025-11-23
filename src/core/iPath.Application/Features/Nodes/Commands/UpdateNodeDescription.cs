using DispatchR.Abstractions.Send;

namespace iPath.Application.Features.Nodes;

public record UpdateNodeCommand(Guid NodeId, NodeDescription? Description, bool? IsDraft)
    : IRequest<UpdateNodeCommand, Task<bool>>
    , IEventInput
{
    public string ObjectName => nameof(Node);
}

public static partial class NodeCommandExtensions
{
    public static Node UpdateNode(this Node node, UpdateNodeCommand request, Guid userId)
    {
        bool isPubish = false;
        if (node.IsDraft && request.IsDraft.HasValue && !request.IsDraft.Value)
            isPubish = true;

        if (request.Description is not null)
            node.Description = request.Description;
        if (request.IsDraft.HasValue)
            node.IsDraft = request.IsDraft.Value;

        node.LastModifiedOn = DateTime.UtcNow;
        node.CreateEvent<NodeDescriptionUpdatedEvent, UpdateNodeCommand>(request, userId);
        if (isPubish)
        {
            node.CreateEvent<RootNodePublishedEvent, UpdateNodeCommand>(request, userId);
        }

        return node;
    }
}